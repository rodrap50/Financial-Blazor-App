using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;


namespace Financial.Api.Infrastructure.Controllers;
using Financial.Shared;
using Infrastructure;
using Microsoft.Azure.Functions.Worker;
using System;

// Functions must return IActionResult under the ASP.NET Core integration model; a bare POCO
// return value is not written to the HTTP response body.
public class AccountFunctions(IAccountService accountService)
{
    [Function("GetAllAccounts")]
    public async Task<IActionResult> GetAllAccounts([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        return new OkObjectResult(await accountService.GetAllAccountsAsync());
    }

    [Function("GetAccountById")]
    public async Task<IActionResult> GetAccountById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "account/{accountId}")] HttpRequest req, string accountId)
    {
        var account = await accountService.GetAccountByIdAsync(Guid.Parse(accountId));
        return account is null ? new NotFoundResult() : new OkObjectResult(account);
    }
    [Function("CreateAccount")]
    public async Task<IActionResult> CreateAccount([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        var account = await req.ReadFromJsonAsync<Account>();
        if (account is null)
        {
            return new BadRequestObjectResult("Request body must be an account.");
        }
        var created = await accountService.CreateAccountAsync(account);
        return new CreatedResult($"account/{created.Id}", created);
    }
    [Function("UpdateAccount")]
    public async Task<IActionResult> UpdateAccount([HttpTrigger(AuthorizationLevel.Function, "put", Route = "account/{accountId}")] HttpRequest req, string accountId)
    {
        var account = await req.ReadFromJsonAsync<Account>();
        if (account is null)
        {
            return new BadRequestObjectResult("Request body must be an account.");
        }
        if (!await accountService.AccountExistsAsync(Guid.Parse(accountId)))
        {
            return new NotFoundResult();
        }
        return new OkObjectResult(await accountService.UpdateAccountAsync(account));
    }
    [Function("DeleteAccount")]
    public async Task<IActionResult> DeleteAccount([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "account/{accountId}")] HttpRequest req, string accountId)
    {
        if (!await accountService.AccountExistsAsync(Guid.Parse(accountId)))
        {
            return new NotFoundResult();
        }
        return new OkObjectResult(await accountService.DeleteAccountAsync(Guid.Parse(accountId)));
    }
    [Function("DeleteAllAccounts")]
    public async Task<IActionResult> DeleteAllAccounts([HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req)
    {
        return new OkObjectResult(await accountService.DeleteAllAccounts());
    }

}
