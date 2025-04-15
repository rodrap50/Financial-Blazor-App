using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;


namespace Financial.Api.Infrastructure.Controllers;
using Data;
using Infrastructure;
using Microsoft.Azure.Functions.Worker;
using System;

public class AccountFunctions(IAccountService accountService)
{
    [Function("GetAllAccounts")]
    public async Task<IEnumerable<Account>> GetAllAccounts([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        return await accountService.GetAllAccountsAsync();
    }

    [Function("GetAccountById")]
    public async Task<Account> GetAccountById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "account/{accountId}")] HttpRequest req, string accountId)
    {
        return await accountService.GetAccountByIdAsync(Guid.Parse(accountId));
    }
    [Function("CreateAccount")]
    public async Task<Account> CreateAccount([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
        //var body = await req.ReadAsStringAsync();
        var account = await req.ReadFromJsonAsync<Account>();
        //var account = JsonSerializer.Deserialize<Account>(body);
        return await accountService.CreateAccountAsync(account);
    }
    [Function("UpdateAccount")]
    public async Task<Account> UpdateAccount([HttpTrigger(AuthorizationLevel.Function, "put", Route = "account/{accountId}")] HttpRequest req, string accountId)
    {
        //var body = await req.ReadAsStringAsync();
        //var account = JsonSerializer.Deserialize<Account>(body);
        var account = await req.ReadFromJsonAsync<Account>();
        if (accountService.AccountExistsAsync(Guid.Parse(accountId)).Result == false)
        {
            throw new ArgumentException("Account does not exist.", nameof(accountId));
        }
        return await accountService.UpdateAccountAsync(account);
    }
    [Function("DeleteAccount")]
    public async Task<bool> DeleteAccount([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "account/{accountId}")] HttpRequest req, string accountId)
    {
        if (accountService.AccountExistsAsync(Guid.Parse(accountId)).Result == false)
        {
            throw new ArgumentException("Account does not exist.", nameof(accountId));
        }
        return await accountService.DeleteAccountAsync(Guid.Parse(accountId));
    }
    [Function("DeleteAllAccounts")]
    public async Task<bool> DeleteAllAccounts([HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req)
    {
        return await accountService.DeleteAllAccounts();
    }

}
