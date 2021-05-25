using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace FinancialApp.Data
{
    public class EventEntry
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString(format: "D");

        [JsonProperty(PropertyName = "eventName")]
        public string EventName { get; set; }
    }
}