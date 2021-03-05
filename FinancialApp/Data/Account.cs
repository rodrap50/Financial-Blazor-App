using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FinancialApp.Data
{
    public class Account {
        [JsonProperty(PropertyName = "id")]
        public string Id {get; set; }

        [JsonProperty(PropertyName = "recordId")]
        public string RecordId {get; set;}

        [JsonProperty(PropertyName = "accountName")]
        public string AccountName {get; set;}

        [JsonProperty(PropertyName = "balance")]
        public decimal Balance {get; set;}        
    }
}