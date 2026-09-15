# Task Plan — Financial Blazor App

> Task board for the current modernization work. Full ticket detail (files touched, acceptance
> criteria, status) lives in [TICKETS.md](TICKETS.md).
> See [ROADMAP.md](ROADMAP.md) for the phase-level picture.
> Updated at the end of each session.

---

## Sprint 1 — API Layer + Blazor Connectivity

### API Layer

| # | Task | Status | Summary | Blocked By |
|---|---|---|---|---|
| T01 | Add Events and Transactions to CosmosDbContext | ✅ Done | `Events`/`Transactions` DbSets added; mappings moved into `IEntityTypeConfiguration` classes under `Data/Configuration/`. Done 2026-09-15. | — |
| T02 | Add IEventService and EventService | 🔲 Pending | Full CRUD service layer for `FinancialEvent` | — |
| T03 | Add EventFunctions HTTP triggers | 🔲 Pending | Expose event CRUD as Azure Functions HTTP triggers | T02 |
| T04 | Add ITransactionService and TransactionService | 🔲 Pending | Transaction service layer, including account balance updates | — |
| T05 | Add TransactionFunctions HTTP triggers | 🔲 Pending | Expose transaction CRUD/queries as HTTP triggers | T04 |
| T06 | Wire DatabaseInitializer to API startup | ✅ Done | Called in `Program.cs` after `Build()`; logs + rethrows on failure. Done 2026-09-15 during the .NET 10 prototype (see MODERNIZATION.md). | — |

### Blazor App

| # | Task | Status | Summary | Blocked By |
|---|---|---|---|---|
| T07 | Configure Blazor HttpClient to point at the API | 🔲 Pending | Configured `HttpClient` with `ApiBaseUrl` for dev and prod | — |
| T08 | Update Accounts page to correct API routes | 🔲 Pending | Fix Accounts page to use configured client and correct route | T07 |
| T09 | Build Events page connected to API | 🔲 Pending | Events list + edit pages calling real `EventFunctions` | T07, T03 |
| T10 | Build Transactions page connected to API | 🔲 Pending | Transaction list + edit pages with account/event filtering | T07, T05 |
| T11 | Build NewEntry page connected to API | 🔲 Pending | NewEntry page posts transactions through the API | T07, T05 |

Full detail for T01-T11: [TICKETS.md](TICKETS.md#sprint-1--api-layer-tickets)

---

## Modernization Next Steps

> Unscheduled backlog carried over from MODERNIZATION.md's "Suggested next steps". Not part of
> Sprint 1's connectivity scope.

| # | Task | Status | Summary | Blocked By |
|---|---|---|---|---|
| T12 | Commit the pending uncommitted diff | 🔲 Pending | Review `git status`/`git diff` and commit the documented modernization work in logical chunks | — |
| T13 | Decide scope and timing for the Events/Transactions backend | 🔲 Pending | Decision checkpoint on whether/when T01-T05 proceed as scoped | — |
| T14 | Provision Azure resources and deployment workflow | 🔲 Pending | Stand up SWA + Cosmos Serverless + GitHub Actions deploy (ROADMAP Phase 3) | — |
| T15 | Add test coverage and CI | 🔲 Pending | Unit/integration tests and a CI build+test step (ROADMAP Phase 4) | — |

Full detail for T12-T15: [TICKETS.md](TICKETS.md#modernization-next-steps-tickets)

---

## Status Key

| Symbol | Meaning |
|---|---|
| 🔲 Pending | Not started |
| 🔄 In Progress | Actively being built |
| ✅ Done | Complete |
| ⏸ Deferred | Moved to post-MVP |
