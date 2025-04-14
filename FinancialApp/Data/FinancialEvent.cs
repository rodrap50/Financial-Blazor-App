using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FinancialApp.Data
{
    public class FinancialEvent
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "recordId")]
        public string RecordId { get; set; }

        [JsonProperty(PropertyName = "eventName")]
        public string EventName { get; set; }
        [JsonProperty(PropertyName = "balance")]
        public decimal Balance { get; set; }
    }
}
