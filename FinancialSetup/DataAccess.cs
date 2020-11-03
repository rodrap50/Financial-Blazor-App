using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;

namespace FinancialSetup
{
    class DataAccess
    {
        public const string CONTAINER_NAME = "Finances";
        public const string CONTAINER_INDEX = "/Record";

        private string endPointURL;
        private string connectivityKey;
        private string databaseName;

        private CosmosClient cosmosClient;
        private Database database;
        private Container container;

        public DataAccess(string endPointURL, string connectivityKey, string databaseName)
        {
            this.endPointURL = endPointURL;
            this.connectivityKey = connectivityKey;
            this.databaseName = databaseName;
        }

        public async Task Connect()
        {
            this.cosmosClient = new CosmosClient(endPointURL, connectivityKey, null);
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(this.databaseName);
            this.container = await this.database.CreateContainerIfNotExistsAsync(CONTAINER_NAME,
                                                                                 CONTAINER_INDEX);

        }

        public async Task AddItem(FinancialEntry entry)
        {
            ItemResponse<FinancialEntry> entryResponse = await this.container.CreateItemAsync<FinancialEntry>(entry, new PartitionKey(entry.Record));
        }

        public async Task AddEvent(FinancialEvent entry)
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.Record = 'eventlist'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<FinancialEventList> queryResultSetIterator = this.container.GetItemQueryIterator<FinancialEventList>(queryDefinition);

            List<FinancialEventList> storedEventList = new List<FinancialEventList>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<FinancialEventList> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (FinancialEventList events in currentResultSet)
                {
                    storedEventList.Add(events);
                }
            }

            FinancialEventList eventList;
            bool newList = false;
            if (storedEventList.Count == 1)
            {
                eventList = storedEventList[0];
            }
            else
            {
                newList = true;
                eventList = new FinancialEventList();
                eventList.Id = Guid.NewGuid().ToString();
            }

            eventList.Events.Add(entry);

            if (newList)
            {
                ItemResponse<FinancialEventList> entryResponse = await this.container.CreateItemAsync(eventList, new PartitionKey(eventList.Record));
            }
            else
            {
                ItemResponse<FinancialEventList> entryResponse = await this.container.ReplaceItemAsync(eventList, eventList.Id);
            }
        }



    }
}
