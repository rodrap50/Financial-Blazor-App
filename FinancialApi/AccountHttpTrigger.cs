using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rodrap50.Financial.Api.Models;
using Rodrap50.Financial.Api.Data;
using Rodrap50.Financial.Api.Data.Responses;

namespace Rodrap50.Financial.Api
{
    public static class AccountHttpTrigger
    {
        [FunctionName("GetAccountsSummary")]
        public static IActionResult GetAccountsSummary(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "accounts")] HttpRequest req,
             [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "1000",
                PartitionKey = "accountsummary")] AccountsResponse accounts,
                ILogger log)
        {
            log.LogInformation("C# HTTP GetAccounts trigger function processed a request.");

            return new OkObjectResult(accounts);
        }

        [FunctionName("GetAccount")]
        public static IActionResult GetAccount( [HttpTrigger(AuthorizationLevel.Function, "get", Route = "account/{id}")] HttpRequest req,
             [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{id}",
                PartitionKey = "account")] AccountResponse account,
                ILogger log){

                     log.LogInformation("C# HTTP GetAccount trigger function processed a request.");

            return new OkObjectResult(account);
        }

        [FunctionName("UpdateAccountDetails")]
        public static async Task<IActionResult> UpdateAccountDetails(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "account/{id}")] HttpRequest request, 
            [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
                ILogger logger
        ) {
            logger.LogInformation("C# HTTP CreateAccount trigger function processed a request.");
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var account = JsonConvert.DeserializeObject<Account>(requestBody);
            
            StoredProcedureResponse<Account> sprocResponse = await client.ExecuteStoredProcedureAsync<Account>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/UpdateAccountDetails/", new RequestOptions { PartitionKey = new PartitionKey("account") }, account);

            account = sprocResponse.Response;
   
            StoredProcedureResponse<AccountsResponse> sprocResponse2 = await client.ExecuteStoredProcedureAsync<AccountsResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/UpdateAccountSummary/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") }, account);


            return new OkObjectResult(account);
        }



        [FunctionName("CreateAccount")]
        public static async Task<IActionResult> CreateAccount(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route="account")] HttpRequest request, 
            [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
                ILogger logger
        ) {
            logger.LogInformation("C# HTTP CreateAccount trigger function processed a request.");
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<AccountCreateModel>(requestBody);
            var account = input.GenerateAccount();

            StoredProcedureResponse<ReserveNextAccountResponse> sprocResponse = await client.ExecuteStoredProcedureAsync<ReserveNextAccountResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/ReserveNextAccount/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") });

            account.RecordId = sprocResponse.Response.AccountId;

            Document document = await client.CreateDocumentAsync("/dbs/Rodrap50/colls/Financials/", account);
    
            StoredProcedureResponse<AccountsResponse> sprocResponse2 = await client.ExecuteStoredProcedureAsync<AccountsResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/UpdateAccountSummary/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") }, account);


            return new OkObjectResult(document);
        }

        
    }
}
