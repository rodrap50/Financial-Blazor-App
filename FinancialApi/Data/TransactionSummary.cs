using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using Rodrap50.Financial.Api.Data.Base;

namespace Rodrap50.Financial.Api.Data
{
    public class TransactionSummary
    {
        [JsonProperty(PropertyName = "id")]
        public string Id {get; set; } = Guid.NewGuid().ToString(format: "D");
        [JsonProperty(PropertyName = "recordId")]
        public string RecordId { get; set; }
        [JsonProperty(PropertyName = "dateIncurred")]
        public DateTime DateIncurred { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }
    }
}

