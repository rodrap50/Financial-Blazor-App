using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Rodrap50.Financial.Api.Data.Base;

namespace Rodrap50.Financial.Api.Data
{
    public class FinancialEvent : EventBase {

        [JsonProperty(PropertyName = "transactions")]
        public List<TransactionEntry> Transactions {get; set;}
        [JsonProperty(PropertyName = "transactionSummary")]
        public List<Transaction> TransactionSummary { get; set; }             
    }

}