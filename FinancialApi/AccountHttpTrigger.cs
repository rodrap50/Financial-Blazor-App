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
using Rodrap50.Financial.Api.Data.Base;
using Rodrap50.Financial.Api.Data.Responses;
using Rodrap50.Financial.Api.Data.Requests;

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


        [FunctionName("GetListings")]
        public static IActionResult GetListings(
                [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listings")] HttpRequest req,
                 [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "1001",
                PartitionKey = "accountsummary")] ListingsResponse listings,
                    ILogger log)
        {
            log.LogInformation("C# HTTP GetListings trigger function processed a request.");

            return new OkObjectResult(listings);
        }

        [FunctionName("GetAccount")]
        public static IActionResult GetAccount([HttpTrigger(AuthorizationLevel.Function, "get", Route = "account/{id}")] HttpRequest req,
             [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{id}",
                PartitionKey = "account")] AccountResponse account,
                ILogger log)
        {

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
        )
        {
            logger.LogInformation("C# HTTP CreateAccount trigger function processed a request.");
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var account = JsonConvert.DeserializeObject<Account>(requestBody);

            StoredProcedureResponse<Account> sprocResponse = await client.ExecuteStoredProcedureAsync<Account>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/UpdateAccountDetails/", new RequestOptions { PartitionKey = new PartitionKey("account") }, account);

            account = sprocResponse.Response;

            AccountsRequest summaryRequest = new AccountsRequest();
            summaryRequest.accounts = new Account[]{account};

            StoredProcedureResponse<AccountsResponse> sprocResponse2 = await client.ExecuteStoredProcedureAsync<AccountsResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/UpdateAccountSummary/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") }, summaryRequest);

            Account generalAccount = null;

            if (account.SoftAccount)
            {
                var docUri = UriFactory.CreateDocumentUri("Rodrap50", "Financials", account.GeneralAccountId);
                generalAccount = await client.ReadDocumentAsync<Account>(docUri);
            }
            else
            {
                generalAccount = account;
            }

            StoredProcedureResponse<ListingsResponse> sprocResponse3 = await client.ExecuteStoredProcedureAsync<ListingsResponse>(
                                                               "/dbs/Rodrap50/colls/Financials/sprocs/UpdateAccountListing/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") }, generalAccount);

            return new OkObjectResult(account);
        }

        /* 
        [FunctionName("DeleteAccount")]
        */

        /*
        [FunctionName("CloseAccount")]
        */



        [FunctionName("CreateAccount")]
        public static async Task<IActionResult> CreateAccount(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "account")] HttpRequest request,
            [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
                ILogger logger
        )
        {
            logger.LogInformation("C# HTTP CreateAccount trigger function processed a request.");
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<AccountCreateModel>(requestBody);
            var account = input.GenerateAccount();

            StoredProcedureResponse<ReserveNextAccountResponse> sprocResponse = await client.ExecuteStoredProcedureAsync<ReserveNextAccountResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/ReserveNextAccount/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") });

            account.RecordId = sprocResponse.Response.AccountId;

            StoredProcedureResponse<AccountResponse> sprocResponse1 = await client.ExecuteStoredProcedureAsync<AccountResponse>("/dbs/Rodrap50/colls/Financials/sprocs/AddAccount/", new RequestOptions { PartitionKey = new PartitionKey("account") }, account);

            AccountsRequest summaryRequest = new AccountsRequest();
            summaryRequest.accounts = new Account[]{account};

            StoredProcedureResponse<AccountsResponse> sprocResponse2 = await client.ExecuteStoredProcedureAsync<AccountsResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/UpdateAccountSummary/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") },summaryRequest);

            Account generalAccount = null;

            if (sprocResponse1.Response.SoftAccount)
            {
                var docUri = UriFactory.CreateDocumentUri("Rodrap50", "Financials", sprocResponse1.Response.GeneralAccountId);
                generalAccount = await client.ReadDocumentAsync<Account>(docUri,  new RequestOptions { PartitionKey = new PartitionKey("account") });
            }
            else
            {
                generalAccount = sprocResponse1.Response;
            }

            StoredProcedureResponse<ListingsResponse> sprocResponse3 = await client.ExecuteStoredProcedureAsync<ListingsResponse>(
                                                               "/dbs/Rodrap50/colls/Financials/sprocs/UpdateAccountListing/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") }, generalAccount);


            return new OkObjectResult(sprocResponse1.Response);
        }

        [FunctionName("CreateEvent")]
        public static async Task<IActionResult> CreateEvent([HttpTrigger(AuthorizationLevel.Function, "put", Route = "event")] HttpRequest request,
            [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
                ILogger logger

        )
        {
            logger.LogInformation("C# HTTP CreateEvent trigger function processed a request.");
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<EventCreateModel>(requestBody);
            var financialEvent = input.GenerateEvent();

            StoredProcedureResponse<ReserveNextEventResponse> sprocResponse = await client.ExecuteStoredProcedureAsync<ReserveNextEventResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/ReserveNextEvent/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") });

            financialEvent.RecordId = sprocResponse.Response.EventId;

            Document document = await client.CreateDocumentAsync("/dbs/Rodrap50/colls/Financials/", financialEvent);

            

            StoredProcedureResponse<AccountsResponse> sprocResponse2 = await client.ExecuteStoredProcedureAsync<AccountsResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/UpdateEventSummary/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") }, financialEvent);


            StoredProcedureResponse<ListingsResponse> sprocResponse3 = await client.ExecuteStoredProcedureAsync<ListingsResponse>(
                                                               "/dbs/Rodrap50/colls/Financials/sprocs/UpdateEventListing/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") }, financialEvent);


            return new OkObjectResult(document);

        }

        [FunctionName("UpdateEventDetails")]
        public static async Task<IActionResult> UpdateEventDetails(
           [HttpTrigger(AuthorizationLevel.Function, "put", Route = "event/{id}")] HttpRequest request,
           [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
               ILogger logger
       )
        {
            logger.LogInformation("C# HTTP UpdateEventDetails trigger function processed a request.");
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var individualEvent = JsonConvert.DeserializeObject<FinancialEvent>(requestBody);

            StoredProcedureResponse<FinancialEvent> sprocResponse = await client.ExecuteStoredProcedureAsync<FinancialEvent>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/UpdateEventDetails/", new RequestOptions { PartitionKey = new PartitionKey("event") }, individualEvent);

            individualEvent = sprocResponse.Response;

            StoredProcedureResponse<AccountsResponse> sprocResponse2 = await client.ExecuteStoredProcedureAsync<AccountsResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/UpdateEventSummary/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") }, individualEvent);

StoredProcedureResponse<ListingsResponse> sprocResponse3 = await client.ExecuteStoredProcedureAsync<ListingsResponse>(
                                                               "/dbs/Rodrap50/colls/Financials/sprocs/UpdateEventListing/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") }, individualEvent);

            return new OkObjectResult(individualEvent);
        }

        [FunctionName("GetEvent")]
        public static IActionResult GetEvent([HttpTrigger(AuthorizationLevel.Function, "get", Route = "event/{id}")] HttpRequest req,
                    [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{id}",
                PartitionKey = "event")] FinancialEvent eventRecord,
                       ILogger log)
        {

            log.LogInformation("C# HTTP GetEvent trigger function processed a request.");

            return new OkObjectResult(eventRecord);
        }


 [FunctionName("GetTransaction")]
        public static async Task<IActionResult> GetTransaction([HttpTrigger(AuthorizationLevel.Function, "get", Route = "transaction/{id}")] HttpRequest request, string id,
            [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
                ILogger logger

        )
        {
            logger.LogInformation("X# HTTP GetTranssaction trigger function processed a request.");
            var docUri = UriFactory.CreateDocumentUri("Rodrap50", "Financials", id);
            Transaction response = await client.ReadDocumentAsync<Transaction>(docUri, new RequestOptions { PartitionKey = new PartitionKey("transaction")});
            return new OkObjectResult(response);
        }

        [FunctionName("CreateTransaction")]
        public static async Task<IActionResult> CreateTransaction([HttpTrigger(AuthorizationLevel.Function, "put", Route = "transaction")] HttpRequest request,
            [CosmosDB(
                databaseName: "Rodrap50",
                collectionName: "Financials",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
                ILogger logger

        )
        {
            logger.LogInformation("C# HTTP CreateTransaction trigger function processed a request.");
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TransactionCreateModel>(requestBody);
            var transaction = input.GenerateTransaction();

            StoredProcedureResponse<ReserveNextTransactionResponse> sprocResponse = await client.ExecuteStoredProcedureAsync<ReserveNextTransactionResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/ReserveNextTransaction/", new RequestOptions { PartitionKey = new PartitionKey("account") }, transaction.GeneralAccountId);

            transaction.RecordId = sprocResponse.Response.TransactionRecordId;

            Document document = await client.CreateDocumentAsync("/dbs/Rodrap50/colls/Financials/", transaction);

            StoredProcedureResponse<AccountResponse[]> sprocResponse2 = await client.ExecuteStoredProcedureAsync<AccountResponse[]>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/AddAccountTransaction/", new RequestOptions { PartitionKey = new PartitionKey("account") }, transaction);

            AccountsRequest summaryRequest = new AccountsRequest();
            summaryRequest.accounts = sprocResponse2.Response;

            StoredProcedureResponse<AccountsResponse> sprocResponse3 = await client.ExecuteStoredProcedureAsync<AccountsResponse>(
                                                                "/dbs/Rodrap50/colls/Financials/sprocs/UpdateAccountSummary/", new RequestOptions { PartitionKey = new PartitionKey("accountsummary") }, summaryRequest);

            
            return new OkObjectResult(document);


        }
    }
}
