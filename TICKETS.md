# Tickets — Financial Blazor App

> Granular ticket detail (files touched, acceptance criteria, blockers) for every task tracked in
> [TASKS.md](TASKS.md). See [ROADMAP.md](ROADMAP.md) for the phase-level picture and
> [MODERNIZATION.md](MODERNIZATION.md) for the narrative history behind the "Modernization Next
> Steps" tickets below.
> Updated at the end of each session.

---

## Sprint 1 — API Layer Tickets

| # | Ticket | Status | Blocked By |
|---|---|---|---|
| T01 | Add Events and Transactions to CosmosDbContext | ✅ Done — moved to [CompletedTickets.md](CompletedTickets.md#t01--add-events-and-transactions-to-cosmosdbcontext) | — |
| T02 | Add IEventService and EventService | ✅ Done — moved to [CompletedTickets.md](CompletedTickets.md#t02--add-ieventservice-and-eventservice) | — |
| T03 | Add EventFunctions HTTP triggers | ✅ Done — moved to [CompletedTickets.md](CompletedTickets.md#t03--add-eventfunctions-http-triggers) | — |
| T04 | Add ITransactionService and TransactionService | 🔲 Pending | — |
| T05 | Add TransactionFunctions HTTP triggers | 🔲 Pending | T04 |
| T06 | Wire DatabaseInitializer to API startup | ✅ Done — moved to [CompletedTickets.md](CompletedTickets.md#t06--wire-databaseinitializer-to-api-startup) | — |

---

### T04 — Add ITransactionService and TransactionService

**Files:** `FinancialApi/Infrastructure/ITransactionService.cs`, `FinancialApi/Infrastructure/TransactionService.cs`, `FinancialApi/Infrastructure/Startup/ApplicationServiceStartup.cs`

**Status:** 🔲 Pending
**Blocked by:** None — ready to start (T01 done)

**Goal:** Implement the full service layer for `Transaction`, including side-effect logic to keep
account balances consistent when a transaction is created or modified.

**Acceptance criteria:**
- `ITransactionService` declares: `CreateTransactionAsync`, `GetTransactionByIdAsync`,
  `GetTransactionsByAccountAsync`, `GetTransactionsByEventAsync`, `UpdateTransactionAsync`,
  `DeleteTransactionAsync`
- `TransactionService` implements all six methods using `CosmosDbContext`
- `CreateTransactionAsync` also updates the affected `Account`'s balance (credit/debit applied
  based on transaction direction)
- `TransactionService` is registered in `ApplicationServiceStartup` as `ITransactionService`
- `dotnet build FinancialApi.sln` passes with no new errors

---

### T05 — Add TransactionFunctions HTTP triggers

**File:** `FinancialApi/Functions/TransactionFunctions.cs`

**Status:** 🔲 Pending
**Blocked by:** T04. Follow `EventFunctions` (T03): explicit lowercase `Route`, `Guid.TryParse` →
400, service exceptions mapped to 404 / 409, `Created(uri, value)` for 201 (not `CreatedAtAction`,
which needs an MVC route table the worker does not have).

**Goal:** Expose `ITransactionService` operations as Azure Functions v4 HTTP triggers.

**Acceptance criteria:**
- Six HTTP triggers implemented: `GetTransactionById`, `GetTransactionsByAccount`,
  `GetTransactionsByEvent`, `CreateTransaction`, `UpdateTransaction`, `DeleteTransaction`
- Routes follow a consistent, lowercase pattern (e.g., `api/transactions`, `api/transactions/{id}`,
  `api/transactions/account/{accountId}`, `api/transactions/event/{eventId}`)
- Each trigger returns appropriate HTTP status codes and JSON bodies
- Functions host starts without error
- All six routes return expected responses when called via curl or a REST client against the local
  Functions host

---

## Sprint 1 — Blazor App Tickets

| # | Ticket | Status | Blocked By |
|---|---|---|---|
| T07 | Configure Blazor HttpClient to point at the API | 🔲 Pending | — |
| T08 | Update Accounts page to correct API routes | 🔲 Pending | T07 |
| T09 | Build Events page connected to API | 🔲 Pending | T07 |
| T10 | Build Transactions page connected to API | 🔲 Pending | T07, T05 |
| T11 | Build NewEntry page connected to API | 🔲 Pending | T07, T05 |

---

### T07 — Configure Blazor HttpClient to point at the API

**Files:** `FinancialApp/wwwroot/appsettings.json`, `FinancialApp/wwwroot/appsettings.Development.json`, `FinancialApp/Program.cs`

**Status:** 🔲 Pending
**Blocked by:** None — ready to start

**Goal:** Replace any hardcoded URLs and the bare default `HttpClient` with a properly configured
client that points at the Azure Functions API in both local dev and production.

**Acceptance criteria:**
- `appsettings.json` contains an `ApiBaseUrl` key set to the production SWA `/api` base path
- `appsettings.Development.json` contains `ApiBaseUrl` set to the local Functions host URL
  (e.g., `http://localhost:7071`)
- `Program.cs` reads `ApiBaseUrl` from configuration and registers a named or typed `HttpClient`
  with that base address
- No hardcoded URLs remain in `Program.cs` or any Razor page
- `dotnet build FinancialApi.sln` passes with no new errors

---

### T08 — Update Accounts page to correct API routes

**File:** `FinancialApp/Pages/Accounts.razor`

**Status:** 🔲 Pending
**Blocked by:** T07

**Goal:** Make the Accounts page use the configured API `HttpClient` and correct function route
so it actually loads account data in both local dev and production.

**Acceptance criteria:**
- `Accounts.razor` injects the configured `HttpClient` (not the bare default client)
- The HTTP call targets the correct route matching `AccountFunctions` (e.g., `api/GetAllAccounts`
  or whatever canonical route is confirmed in `AccountFunctions.cs`)
- The page renders a list of accounts when the API returns data
- The page displays a user-visible error or empty state (not an unhandled exception) when the API
  is unreachable

---

### T09 — Build Events page connected to API

**Files:** `FinancialApp/Pages/Events.razor` (or equivalent list page), `FinancialApp/Pages/EditEvent.razor`

**Status:** 🔲 Pending
**Blocked by:** T07 (T03 done — live routes are `api/events` and `api/events/{eventId}`)

**Goal:** Events.razor lists all events from the API; EditEvent.razor loads a single event by ID
and saves changes back via the API.

**Acceptance criteria:**
- Events list page calls `GET api/events` and renders event name/date for each result
- EditEvent page loads an existing event by ID (`GET api/events/{id}`) when an ID is provided
- EditEvent page posts a new event (`POST api/events`) when no ID is provided (new entry)
- EditEvent page updates an existing event (`PUT api/events/{id}`) on save
- Both pages display a user-visible error state when the API call fails
- No calls target routes that do not exist in `EventFunctions`

---

### T10 — Build Transactions page connected to API

**Files:** `FinancialApp/Pages/EditTransactions.razor` (and list page if separate)

**Status:** 🔲 Pending
**Blocked by:** T07, T05

**Goal:** Transaction pages load and display transactions from the API, with filtering support by
account or event.

**Acceptance criteria:**
- Transaction list can be filtered by account ID or event ID via the appropriate API routes
- Each transaction row displays: date, amount, direction (credit/debit), transaction method,
  account, event (if linked)
- EditTransactions page loads an existing transaction by ID and allows editing
- Save calls `PUT api/transactions/{id}`; delete calls `DELETE api/transactions/{id}`
- Both pages display a user-visible error state when the API call fails

---

### T11 — Build NewEntry page connected to API

**File:** `FinancialApp/Pages/NewEntry.razor`

**Status:** 🔲 Pending
**Blocked by:** T07, T05

**Goal:** NewEntry.razor submits a new transaction through the API with all required fields.

**Acceptance criteria:**
- Form captures: account (dropdown), event (optional dropdown), amount, direction (credit/debit),
  transaction method, date
- On submit, calls `POST api/transactions` with the correct payload shape matching
  `TransactionFunctions.CreateTransaction`
- On success, the form resets or navigates to the transaction list with a confirmation message
- On failure, an error message is displayed inline (no unhandled exception)
- No dummy/hardcoded data is submitted; all fields come from user input or dropdowns populated
  from the API

---

## Modernization Next Steps Tickets

> Sourced from MODERNIZATION.md's "Suggested next steps" section. These are not part of Sprint 1's
> API/Blazor connectivity scope — they cover repo hygiene, scope decisions, and the deployment/quality
> phases (ROADMAP.md Phases 3-4).

| # | Ticket | Status | Blocked By |
|---|---|---|---|
| T12 | Commit the pending uncommitted diff | 🔲 Pending | — |
| T13 | Decide scope and timing for the Events/Transactions backend | 🔲 Pending | — |
| T14 | Provision Azure resources and deployment workflow | 🔲 Pending | — |
| T15 | Add test coverage and CI | 🔲 Pending | — |
| T16 | Centralize Blazor error handling | 🔲 Pending | — |

---

### T12 — Commit the pending uncommitted diff

**Status:** 🔲 Pending
**Blocked by:** None — ready to start

**Goal:** MODERNIZATION.md documents a large body of completed-but-uncommitted work (dead scaffolding
removal, net10.0 retargeting, Functions host bootstrap fix, `Financial.Shared` extraction, Cosmos/Account
bug fixes, Blazor Server → WASM conversion, `staticwebapp.config.json`). Review it with `git status` /
`git diff` and commit it in whatever logical chunks make sense so it isn't at risk of being lost.

**Acceptance criteria:**
- `git status` shows a clean working tree (or only intentionally-remaining untracked files)
- The uncommitted work described in MODERNIZATION.md's "Done, uncommitted" section is captured in one
  or more commits with messages that describe what changed and why
- No unrelated or accidental files (secrets, local settings with real credentials, build output) are
  included in the commits

---

### T13 — Decide scope and timing for the Events/Transactions backend

**Status:** 🔲 Pending
**Blocked by:** None — ready to start

**Goal:** MODERNIZATION.md notes the Events/Transactions backend (`EventFunctions`/`TransactionFunctions`
and their service methods) is explicitly deferred, with a legacy DocumentDB-based implementation
available for reference in `AccountHttpTrigger.cs` but not directly reusable (targets the old SDK and
stored-procedure architecture). This ticket is a decision checkpoint, not implementation: confirm
whether/when T01-T05 (the Events/Transactions API tickets already tracked above) should be picked up,
or whether the scope should change.

**Acceptance criteria:**
- A decision is recorded (in ROADMAP.md, TASKS.md, or here) on whether T01-T05 proceed as scoped,
  are re-scoped, or are deferred to a later phase
- If re-scoped or deferred, ROADMAP.md Phase 1 and/or TASKS.md Sprint 1 are updated to reflect the
  decision
- The legacy commented-out implementation in `AccountHttpTrigger.cs` is either confirmed as reference
  material to keep, or flagged for removal, as part of this decision

---

### T14 — Provision Azure resources and deployment workflow

**Status:** 🔲 Pending
**Blocked by:** None — ready to start (independent of API/Blazor completion, but only delivers value once
those are further along)

**Goal:** Corresponds to ROADMAP.md Phase 3. Stand up the actual Azure resources (Static Web App,
Cosmos DB in Serverless capacity mode) and a deploy workflow, once ready to go beyond local dev.

**Acceptance criteria:**
- Azure Static Web App (Free tier) resource exists with the Functions API linked as Managed Functions
- Azure Cosmos DB account exists, provisioned in Serverless capacity mode
- A GitHub Actions workflow builds and deploys the SWA (frontend + linked API) on push to `master`
- `local.settings.json` values needed for production are documented, or a secrets management approach
  is decided and written down

---

### T15 — Add test coverage and CI

**Status:** 🔲 Pending
**Blocked by:** None — ready to start

**Goal:** Corresponds to ROADMAP.md Phase 4. MODERNIZATION.md notes zero test coverage and an empty
`.github/workflows` directory. Add tests and a CI build/test step when ready to invest there.

**Acceptance criteria:**
- Unit tests exist for the service layer (`AccountService`, and `EventService`/`TransactionService`
  once built)
- At least one integration or smoke test exists for a key HTTP trigger
- A CI workflow (GitHub Actions) runs build + test on pull request

---

### T16 — Centralize Blazor error handling

**Files:** `FinancialApp/Shared/MainLayout.razor` (or `App.razor`), `FinancialApp/Program.cs`, and the
pages that currently catch API errors themselves
(`Accounts.razor`, `Events.razor`, `EditEvent.razor`, and later `EditTransactions.razor` / `NewEntry.razor`)

**Status:** 🔲 Pending
**Blocked by:** None — ready to start. Best done once T10/T11 land, so every page that calls the API
can be moved over in one pass.

**Goal:** T08 and T09 handle API failures page by page: each page wraps its calls in
`try/catch (Exception)`, keeps its own `error` string, and renders its own `alert-danger`. That
pattern will be copied into every new page. Replace it with one app-wide mechanism, so a failed API
call is reported in a consistent place and pages only contain their happy path.

**Acceptance criteria:**
- One shared mechanism (e.g. a scoped error service read by `MainLayout`, a `DelegatingHandler` on the
  API `HttpClient`, an `<ErrorBoundary>`, or a combination) handles and displays API failures
- The message is consistent across pages and includes the status code / server message for non-2xx
  responses
- Pages no longer declare their own `error` field or catch-all `try/catch` around API calls
- A failed save still leaves the user on the form with their input intact (the current EditEvent
  behaviour)
- Unhandled render exceptions no longer fall through to the default Blazor "An unhandled error has
  occurred" bar without a useful message

---

## Status Key

| Symbol | Meaning |
|---|---|
| 🔲 Pending | Not started |
| 🔄 In Progress | Actively being built |
| ✅ Done | Complete |
| ⏸ Deferred | Moved to post-MVP |
