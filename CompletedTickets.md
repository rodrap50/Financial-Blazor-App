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

---

## Sprint 1 — API Layer

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
