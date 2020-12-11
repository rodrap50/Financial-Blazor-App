using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Rodrap50.Financial.Api.Data.Base
{
    public class AccountBase {
        [JsonProperty(PropertyName = "id")]
        public string Id {get; set; } = Guid.NewGuid().ToString(format: "D");

       

        [JsonProperty(PropertyName = "recordId")]
        public string RecordId {get; set;}

        [JsonProperty(PropertyName = "accountName")]
        public string AccountName {get; set;}

        [JsonProperty(PropertyName = "balance")]
        public decimal Balance {get; set;}        
    }
}