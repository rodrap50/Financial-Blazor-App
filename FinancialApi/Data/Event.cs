using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rodrap50.Financial.Api.Data
{
    public class Event {
        [JsonProperty(PropertyName = "id")]
        public string Id {get; set; } = Guid.NewGuid().ToString(format: "N");
        public string RecordCode {get; set; } = "event";
        public string RecordId {get; set;}
        public string EventName {get; set;}
        public decimal Balance {get; set;}
        
        public List<string> Transactions {get; set;}
        public List<Transaction> TransactionSummary { get; set; }    
        public string NextTransactionRecordId {get; set;}            
    }
}