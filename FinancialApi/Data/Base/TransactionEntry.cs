using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Financial.Api.Data.Base
{
    public class TransactionEntry {
         [JsonProperty(PropertyName = "id")]
        public string Id {get; set; } = Guid.NewGuid().ToString(format: "D");
       
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount {get; set;}    
    }
}
