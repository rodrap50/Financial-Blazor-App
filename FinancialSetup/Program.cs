using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;

namespace FinancialSetup
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            /// The Azure Cosmos DB endpoint for running this GetStarted sample.
            string EndpointUrl = Environment.GetEnvironmentVariable("EndpointUrl");

            /// The primary key for the Azure DocumentDB account.
            string PrimaryKey = Environment.GetEnvironmentVariable("PrimaryKey");

            /// The database name for the Azure DocumentDB
            string DatabaseName = Environment.GetEnvironmentVariable("FinanceDBName");

             try
       {
           Console.WriteLine("Beginning operations...\n");
           DataAccess backend = new DataAccess(EndpointUrl, PrimaryKey, DatabaseName);
           await backend.Connect();

            Console.WriteLine("Connected to Azure DocumentDB...\n");

           FinancialEntry testEntry = new FinancialEntry();
           testEntry.Id = Guid.NewGuid().ToString();
           testEntry.EventId = "123";
           testEntry.AccountId = "456";
           testEntry.EntryType = FinancialEntryType.Transfer;
           testEntry.TransferToAccountId = "789";
           testEntry.Description = "Test Entry for CRUD";
           testEntry.TransactionDate = DateTime.Now;
           testEntry.Amount = 1.00m;

           await backend.AddItem(testEntry);

           Console.WriteLine("Test Entry saved to Azure DocumentDB...\n");

           FinancialEvent testEvent = new FinancialEvent();
           testEvent.Id = Guid.NewGuid().ToString();
           testEvent.EventName = "Test Event";
           await backend.AddEvent(testEvent);


           

        }
        catch (CosmosException de)
        {
           Exception baseException = de.GetBaseException();
           Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e);
        }
        finally
        {
            Console.WriteLine("End of demo, press any key to exit.");
            Console.ReadKey();
        }
        }
    }
}
