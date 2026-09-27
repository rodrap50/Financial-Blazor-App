# Completed Tickets — Financial Blazor App

> Archive of finished tickets, moved here from [TICKETS.md](TICKETS.md) once done.

---

## Format

When a ticket is completed, move its full entry (summary table row + detail section) here under the
appropriate epic/sprint heading, and record it in the log below.

### Completion Log

| Ticket | Title | Completed | Notes |
|---|---|---|---|
| T06 | Wire DatabaseInitializer to API startup | 2026-09-15 | Done as part of the .NET 10 / Functions prototype (MODERNIZATION.md "Prototype verified") |
| T01 | Add Events and Transactions to CosmosDbContext | 2026-09-15 | Mappings extracted into per-entity `IEntityTypeConfiguration` classes |
| T02 | Add IEventService and EventService | 2026-09-15 | Introduced `NotFoundException` / `ConflictException` for service-layer error signalling |
| T03 | Add EventFunctions HTTP triggers | 2026-09-25 | Function classes moved to `FinancialApi/Functions/`, services to `Infrastructure/Repository/` |

---

## Sprint 1 — API Layer

### T03 — Add EventFunctions HTTP triggers

**Completed:** 2026-09-25

**Summary:** `EventFunctions` exposes all five `IEventService` operations as HTTP triggers on
`api/events` (GET all, POST) and `api/events/{eventId}` (GET, PUT, DELETE). Status codes: 200 on
read/update/delete, 201 + Location on create, 400 for a malformed id or missing body, 404 from
`NotFoundException`, 409 from `ConflictException`, 500 otherwise. `GetEventByIdAsync` was changed to
throw `NotFoundException` rather than return `null`, so all four id-bearing operations signal
absence the same way. Build passes with 0 errors and the host enumerates all five functions.

**Deviations:** the ticket's file path was `Infrastructure/Functions/EventFunctions.cs`; function
classes were instead moved up to `FinancialApi/Functions/` (namespace `Financial.Api.Functions`) and
services down to `Infrastructure/Repository/`, committed separately. `EventFunctions` derives from
`ControllerBase` for its result helpers, unlike `AccountFunctions`. The redundant
`AddScoped<AccountFunctions>()` was dropped — the isolated worker activates function classes
without registration.

**Verified end-to-end** 2026-09-26 against the Docker vNext emulator (host port 8090 → container
8081, `CosmosDbConnectionMode=Gateway`): create 201 + Location, list 200, get 200, update 200 and
persisted, delete 200 then 404, duplicate id 409, unknown id 404, malformed id 400 on all three id
routes, null body 400 on create and update. No errors in the host log.

**Carried forward:** `AccountFunctions` is still on the old `Route = null` convention
(`api/GetAllAccounts`, `api/CreateAccount`) and the `Financial.Api.Infrastructure.Controllers`
namespace — align it during T08. The create `Location` header is `events/{id}`, missing the `api/`
prefix, so it does not resolve to the real route — `AccountFunctions` has the same defect; fix both
together. A `.claude/launch.json` entry named `FinancialApi` runs the host
(`func start --no-build --script-root FinancialApi/bin/Debug/net10.0`).

**Known pitfalls for T05:** an `[HttpTrigger]` must decorate an `HttpRequest` parameter — decorating
the route-value `string` instead makes the generated binding bind the request body, so the route
value never arrives. `CreatedAtAction` resolves its Location through MVC's route table, which the
worker has none of; use `Created(uri, value)`.

**Original ticket detail:**

**File:** `FinancialApi/Infrastructure/Functions/EventFunctions.cs`

**Status:** 🔲 Pending
**Blocked by:** None — ready to start (T02 done). `EventService` throws `NotFoundException` /
`ConflictException` (`Infrastructure/Exceptions/`) — map these to 404 / 409 in the triggers.

**Goal:** Expose `IEventService` operations as Azure Functions v4 HTTP triggers, following the
pattern of `AccountFunctions`.

**Acceptance criteria:**
- Five HTTP triggers implemented: `GetAllEvents`, `GetEventById`, `CreateEvent`, `UpdateEvent`,
  `DeleteEvent`
- Routes follow a consistent, lowercase pattern (e.g., `api/events`, `api/events/{id}`)
- Each trigger returns appropriate HTTP status codes (200, 201, 404, 400) and JSON bodies
- Functions host starts without error (`func start --no-build` from `FinancialApi/bin/Debug/net10.0/`)
- All five routes return expected responses when called via curl or a REST client against the local
  Functions host (Cosmos Emulator must be running for DB-backed assertions)

---

### T02 — Add IEventService and EventService

**Completed:** 2026-09-15

**Summary:** `IEventService` / `EventService` added under `FinancialApi/Infrastructure/` with all six
methods (`CreateEventAsync`, `GetEventByIdAsync`, `GetAllEventsAsync`, `UpdateEventAsync`,
`DeleteEventAsync`, `EventExistsAsync`) backed by `CosmosDbContext.Events`. Registered as scoped in
`ApplicationServiceStartup`. Existence checks use `FirstOrDefaultAsync` rather than `AnyAsync` (the
Docker vNext emulator rejects the `SELECT VALUE EXISTS` that `Any()` emits — same as `AccountService`).
`UpdateEventAsync` pins the entity `Id` to the route id. Deviations: unlike `AccountService` (which
returns `null`/`false`), `EventService` signals errors by throwing `NotFoundException` /
`ConflictException` (new, in `Infrastructure/Exceptions/`) so HTTP triggers can map them to 404/409;
T04 should follow the same convention. Build passes with 0 warnings / 0 errors.

**Original ticket detail:**

**Files:** `FinancialApi/Infrastructure/IEventService.cs`, `FinancialApi/Infrastructure/EventService.cs`, `FinancialApi/Infrastructure/Startup/ApplicationServiceStartup.cs`

**Status:** 🔲 Pending
**Blocked by:** T01

**Goal:** Implement the full CRUD service layer for `FinancialEvent`, following the pattern of
`IAccountService` / `AccountService`.

**Acceptance criteria:**
- `IEventService` declares: `CreateEventAsync`, `GetEventByIdAsync`, `GetAllEventsAsync`,
  `UpdateEventAsync`, `DeleteEventAsync`, `EventExistsAsync`
- `EventService` implements all six methods using `CosmosDbContext`
- `EventService` is registered in `ApplicationServiceStartup` as `IEventService`
- `dotnet build FinancialApi.sln` passes with no new errors

---

### T01 — Add Events and Transactions to CosmosDbContext

**Completed:** 2026-09-15

**Summary:** `CosmosDbContext` now exposes `DbSet<FinancialEvent> Events` and
`DbSet<Transaction> Transactions`. Entity mappings were extracted out of `OnModelCreating` into
per-entity `IEntityTypeConfiguration<T>` classes in `FinancialApi/Data/Configuration/`
(`AccountConfiguration`, `FinancialEventConfiguration`, `TransactionConfiguration`), applied via
`ApplyConfiguration`. Containers: `Accounts`, `FinancialEvents`, `Transactions` — each partitioned on
`RecordCode` with `Id` as the key. Build passes with 0 warnings / 0 errors. Deviations: mapping moved
to configuration classes rather than inline in `OnModelCreating` (cleaner, same model).

**Original ticket detail:**

**File:** `FinancialApi/Data/CosmoDbContext.cs`

**Status:** 🔲 Pending
**Blocked by:** None — ready to start

**Goal:** Extend `CosmosDbContext` so EF Core knows about `FinancialEvent` and `Transaction`,
mirroring the existing `Accounts` container configuration.

**Acceptance criteria:**
- `DbSet<FinancialEvent> Events` property added to `CosmosDbContext`
- `DbSet<Transaction> Transactions` property added to `CosmosDbContext`
- `OnModelCreating` maps each to its own named Cosmos container with a partition key and primary key
  configured (container names and partition key properties match the domain model)
- `dotnet build FinancialApi.sln` passes with no new errors

---

### T06 — Wire DatabaseInitializer to API startup

**Completed:** 2026-09-15

**Summary:** `Program.cs` now resolves `IDatabaseInitializer` from a DI scope after `Build()` and awaits
`InitializeAsync()` before `RunAsync()`. Success logs at Information; failure logs at Critical and
rethrows so the host exits rather than starting without a database. Verified against the Docker Cosmos
vNext emulator — host log shows "Cosmos database initialization complete" and the `Rodrap50` database
plus `Accounts` container were created on first start. Deviations: none.

**Original ticket detail:**

**File:** `FinancialApi/Program.cs`

**Status:** 🔲 Pending
**Blocked by:** None — ready to start

**Goal:** `DatabaseInitializer` is registered in DI but never called. Call `InitializeAsync()` in
`Program.cs` after the host is built and before it starts accepting requests.

**Acceptance criteria:**
- `IDatabaseInitializer.InitializeAsync()` is called in `Program.cs` in the correct position
  (after `Build()`, before `RunAsync()`)
- If initialization fails, the error is logged and the host does not start silently in a broken state
- `dotnet build FinancialApi.sln` passes with no new errors
- Functions host starts and logs a message confirming initialization ran (or a clear error if
  Cosmos is unreachable)
