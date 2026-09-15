# Roadmap — Financial Blazor App

> Treasurer/ledger app for a small organization. One developer. Near-zero Azure cost target.
> Architecture: Blazor WASM + Azure Functions v4 (.NET 10) + EF Core + Cosmos DB Serverless.

---

## Phase 1 — API Layer Completion (Current)

Complete the server-side so all three domain objects (Accounts, Events, Transactions) have working
HTTP endpoints. Account CRUD already exists. This phase fills in the gaps.

Deliverables:
- `FinancialEvent` and `Transaction` added to `CosmosDbContext`
- `IEventService` / `EventService` and `ITransactionService` / `TransactionService` implemented
- `EventFunctions` and `TransactionFunctions` HTTP triggers wired up
- `DatabaseInitializer` called at startup
- All API routes manually testable via the Functions Core Tools local host

---

## Phase 2 — Blazor Connectivity (Current, parallel to Phase 1)

Wire the Blazor WASM frontend to the actual API. Most pages exist as shells but make calls to
routes that do not exist server-side. This phase makes them work.

Deliverables:
- Typed/named `HttpClient` configured against the API base URL in both dev and prod settings
- Accounts page using the correct API client and routes
- Events list + edit pages calling real EventFunctions endpoints
- Transactions list + edit pages calling real TransactionFunctions endpoints
- NewEntry page posting transactions through the API

Note: Phase 2 work on Events and Transactions pages depends on Phase 1 endpoints being available.
The HttpClient configuration and Accounts page fix can start immediately.

---

## Phase 3 — Deployment (Next)

Stand up real Azure resources and a CI/CD pipeline. No code yet.

Deliverables:
- Azure Static Web App (Free tier) with Managed Functions linked
- Azure Cosmos DB account in Serverless capacity mode
- GitHub Actions workflow: build + deploy SWA on push to master
- `local.settings.json` values documented (or a secrets management approach decided)

---

## Phase 4 — Quality (Ongoing, when ready)

Add test coverage and tighten CI. No hard timeline — one developer, small project.

Deliverables:
- Unit tests for service layer (AccountService, EventService, TransactionService)
- Integration/smoke tests for key HTTP triggers
- CI step: build + test on PR

---

## Phase 5 — Member Portal + Authentication (Future / Deferred)

Requires real per-member auth. Azure Static Web Apps has built-in auth (free) that is the
natural fit. Not started; not in scope for the current modernization pass.

Deliverables:
- SWA built-in auth (or Easy Auth) integrated
- Member-facing pages: personal account balance, transaction history, statements
- Per-member `SoftAccount` / `GeneralAccountId` data surface exposed

---

## Phase 6 — Budget Builder (Future / Deferred)

Projection and budgeting tools. Depends on Events/Transactions being stable (Phase 1-2 done).
Scope TBD.

---

## Status Key

| Symbol | Meaning |
|---|---|
| 🔲 Pending | Not started |
| 🔄 In Progress | Actively being built |
| ✅ Done | Complete |
| ⏸ Deferred | Moved to post-MVP |
