using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FinancialApp.Data
{
    public class AccountEntry {
         [JsonProperty(PropertyName = "id")]
        public string Id {get; set; } = Guid.NewGuid().ToString(format: "D");
       
        [JsonProperty(PropertyName = "accountName")]
        public string AccountName {get; set;}  
    }
}