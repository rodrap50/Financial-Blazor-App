# Modernization status

Tracks the in-progress modernization of this app so work can be picked up in a later session.
This file is the source of truth going forward — it supersedes the ad-hoc plan files this work
started from (`~/.claude/plans/recursive-watching-ember.md` and `~/.claude/plans/robust-juggling-lampson.md`
on the machine this was written on), which live outside the repo and aren't portable.

## What this app is

Treasurer/ledger software for a small organization (originally a Boy Scout Troop). One main org
account tracks all funds; each member has an org-maintained sub-account (`SoftAccount` /
`GeneralAccountId` on `AccountBase`) used for event payments and fundraising credits; "Events" group
transactions by occasion/purpose independent of which account the money sits in. Planned but not yet
built: a member self-service portal (sign in, view balance/statements) and a budget builder/projection
tool — both will need real per-member authentication, which doesn't exist yet.

## Target architecture

Chosen for near-zero running cost (~$1/month original estimate) on Azure:

- **API**: Azure Functions, isolated worker model, **.NET 10** (GA Feb 2026, supported through Nov 2028).
- **UI**: Blazor **WebAssembly** (standalone), not Blazor Server — a static bundle fits a scale-to-zero
  cost model; Blazor Server's persistent SignalR connection doesn't.
- **Hosting**: Azure Static Web Apps, **Free tier**, with the Functions API linked as **Managed
  Functions** (same-origin `/api/*`, no CORS, billed at plain Consumption rates). This constrains the
  API to Consumption plan + HTTP-trigger-only, single region — already satisfied, since every function
  in `AccountFunctions.cs` is HTTP-triggered.
- **Data**: Azure Cosmos DB, should be provisioned in **Serverless** capacity mode (pay-per-operation,
  no fixed baseline). This is an Azure Portal/ARM setting, not a code change — `CosmosDbContext` already
  sets no explicit container throughput, so it's already compatible.

## Status

### Done, uncommitted (review with `git status` / `git diff` before committing)

- **Dead scaffolding removed**: `FunctionApp1`, untracked `FunctionApp2`, orphaned `Finance.sln`,
  `.idea`, default Blazor template leftovers (`Counter.razor`, `FetchData.razor`, `WeatherForecast*`,
  `SurveyPrompt.razor`).
- **All projects retargeted to `net10.0`**, package versions aligned to current (dropped in-process-model
  leftovers, added the ASP.NET Core integration package the Functions code actually needs).
- **Functions host bootstrap fixed** (`FinancialApi/Program.cs`): now uses
  `FunctionsApplication.CreateBuilder` + `ConfigureFunctionsWebApplication()`. Also fixed
  `local.settings.json`'s `FUNCTIONS_WORKER_RUNTIME`, which was wrongly set to `"dotnet"` (in-process)
  instead of `"dotnet-isolated"` — the host couldn't discover any functions until that was corrected.
  Deleted `ApplicationLoggingStartup.cs` (dead, never-invoked, wrong abstraction for this pipeline).
- **`Financial.Shared` class library created** (new project, `FinancialShared/`): consolidates
  `Account`, `Transaction`, `FinancialEvent` and their `Base`/`Entry` types (now `System.Text.Json`,
  were `Newtonsoft.Json`) that used to be duplicated independently in `FinancialApi/Data/` and
  `FinancialApp/Data/`. Both projects reference it. UI-only types with real behavior (`IIDEntry`
  interface, `AccountListing`/`ListingsResponse` legacy response wrappers used by not-yet-fixed pages)
  were deliberately kept local to `FinancialApp` rather than merged — see below.
- **Cosmos connection + Account bugs fixed** (`ApplicationServiceStartup.cs`, `AccountService.cs`):
  the Cosmos access key is now actually passed to `UseCosmos` (defaults to the emulator's public
  well-known key for local dev); `GetAccountByIdAsync`/`DeleteAccountAsync` no longer pass a raw `Guid`
  against a `string`-keyed document; `UpdateAccountAsync` is implemented (was
  `NotImplementedException`).
- **`FinancialApp` converted from Blazor Server to Blazor WebAssembly**: new `Program.cs`
  (`WebAssemblyHostBuilder`), `wwwroot/index.html` replacing `Pages/_Host.cshtml`, `Startup.cs` deleted.
  `Accounts.razor` — the one page whose backend feature actually exists — was fixed for real: relative
  `api/GetAllAccounts` call (was a hardcoded, wrong `http://localhost:7071/api/accounts`) and correct
  flat `List<Account>` deserialization (was expecting a legacy wrapped `AccountListing.AccountSummaries`
  shape that the live API never returns).
- **`staticwebapp.config.json` added** at repo root (SPA fallback routing only, so far).

### Known, deliberately left as-is

`Events.razor`, `EditEvent.razor`, `EditTransactions.razor`, `NewEntry.razor` were ported to the new
WASM hosting shell (they compile and run) but their API calls still target routes that **do not exist
anywhere server-side** (`/api/event`, `/api/event/{id}`, `/api/listings`, `/api/transaction` — these
only ever existed in the fully-commented-out legacy `FinancialApi/AccountHttpTrigger.cs`). This was a
deliberate choice, not an oversight: Events/Transactions backend work is on hold. Clicking into these
pages in the running app will fail.

### Explicitly deferred (not started)

- **Events/Transactions backend** — building real `EventFunctions`/`TransactionFunctions` and service
  methods (there's a full legacy implementation commented out in `AccountHttpTrigger.cs` to reference,
  but it targets the old DocumentDB SDK and stored-procedure architecture, not the current EF Core one).
- **Tests and CI** — zero test coverage, empty `.github/workflows`.
- **Static Web Apps deployment automation** — no GitHub Actions workflow, no Azure resources
  provisioned yet (the SWA resource itself, the Cosmos DB Serverless account). Local-only so far.
- **Real authentication** — needed before the member portal can happen; Static Web Apps has built-in
  auth (free) that's a natural fit when this gets picked up.

## Verifying locally

- `dotnet build FinancialApi.sln` — full solution build.
- Functions host: Azure Functions Core Tools has a project-discovery quirk with the SDK-generated
  `obj/**/WorkerExtensions.csproj` — running plain `func start` from `FinancialApi/` fails with
  *"found 2 csproj"*. Workaround:
  ```bash
  cd FinancialApi && dotnet build
  cd bin/Debug/net10.0 && func start --no-build
  ```
- The Cosmos DB Emulator must be running locally (`https://localhost:8081`) for real data — it was not
  installed/running in the sandbox this work was done in, so DB-backed calls were only verified up to
  "reaches the EF Core Cosmos query and fails on the unreachable emulator" (i.e. routing/DI/query code
  confirmed correct, actual data round-trip not yet confirmed against a live Cosmos instance).
- Blazor app: `dotnet run --project FinancialApp` serves the WASM app standalone on
  `http://localhost:5000` — but without a co-located API it can't get real `/api` data (confirmed: the
  relative call resolves same-origin correctly and fails gracefully on the missing backend, matching
  expected behavior). For a real same-origin `/api` proxy matching production, use the
  [Azure Static Web Apps CLI](https://azure.github.io/static-web-apps-cli/) (`swa start`) once installed
  — not available in the sandbox this was built in.

## Suggested next steps

1. Review the uncommitted diff (`git status`, `git diff`) and commit in whatever chunks make sense.
2. Decide the scope/timing for the Events/Transactions backend.
3. Stand up the actual Azure resources (Static Web App, Cosmos DB in Serverless mode) and a deploy
   workflow, once ready to go beyond local dev.
4. Add tests/CI when ready to invest there.

## Prototype verified — 2026-09-15

Goal: confirm the chosen stack (Azure Functions v4, isolated worker, .NET 10, EF Core Cosmos 10) actually
runs end-to-end on this machine before investing further. Result: **it does.**

Context on the "Functions stopped at .NET 6" worry: that was the *in-process* hosting model (retiring
Nov 2026). The *isolated worker* model this project uses supports .NET 8/9/10 on Functions v4.

- Tooling: .NET SDK 10.0.204 / runtime 10.0.8, Azure Functions Core Tools 4.0.5571 (runtime 4.30.0).
- `GET /api/ping` (new, anonymous, no DB) under the Functions host returned
  `{"framework":".NET 10.0.8","runtimeVersion":"10.0.8","workerRuntime":"dotnet-isolated", ...}`.
- Account CRUD round-trip against Cosmos: POST → 201, GET all / GET by id → 200 JSON, PUT → 200,
  DELETE → 200 `true`, missing id → 404. Zero host errors.

### Fixes made while proving it

- `DatabaseInitializer.InitializeAsync()` is now called from `Program.cs` at startup (T06). Failure is
  logged as Critical and rethrown so the host doesn't start in a broken state.
- `AccountFunctions` now return `IActionResult`. Under the ASP.NET Core integration model a bare POCO
  return value is **not** written to the response (`Content-Length: 0`) — this was a pre-existing bug.
  Proper codes now: 201 on create, 404 on missing, 400 on empty body.
- `AccountService` existence checks use `AsNoTracking().FirstOrDefaultAsync` instead of `AnyAsync`
  (see emulator note below), and no-tracking so a following `Update()` doesn't collide.
- `ApplicationServiceStartup` reads `CosmosDbConnectionMode`; `"Gateway"` enables Gateway mode +
  `LimitToEndpoint()` (required by the Docker emulator; leave unset for Direct mode against real Cosmos).

### Local Cosmos: Docker vNext emulator (what worked)

The Windows Cosmos DB Emulator failed here: it regenerated its `DocumentDbEmulatorCertificate` but the
http.sys SSL binding on `0.0.0.0:8081` still pointed at the old cert, so every TLS handshake was reset.
Repairing that binding needs an elevated shell (`netsh http delete/add sslcert`). The Docker emulator
needs no elevation and no certificates:

```bash
docker run -d --name cosmos-vnext -p 8090:8081 -p 1234:1234 \
  mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:vnext-preview --protocol http
```

`local.settings.json` now points at it (`CosmosDbEndpoint=http://localhost:8090`,
`CosmosDbConnectionMode=Gateway`). Data Explorer: http://localhost:1234.

Known vNext-emulator limitation hit: EF Core's `Any()` translates to `SELECT VALUE EXISTS (SELECT 1 FROM
root c ...)`, which the emulator rejects with `Identifier 'root' could not be resolved`. Real Cosmos
accepts it. Prefer `FirstOrDefaultAsync(...) is not null` for existence checks.
