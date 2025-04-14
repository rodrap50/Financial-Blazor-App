using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Financial.Api.Data.Base;

namespace Financial.Api.Data
{
    public class Transaction : TransactionBase {
        [JsonProperty(PropertyName = "id")]
        public string Id {get; set; } = Guid.NewGuid().ToString(format: "D");
        [JsonProperty(PropertyName = "recordCode")]
        public string RecordCode {get; set; } = "transaction";
        
    }

    
}
