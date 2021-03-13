using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Rodrap50.Financial.Api.Data.Base
{
    public class EventBase {
        [JsonProperty(PropertyName = "id")]
        public string Id {get; set; } = Guid.NewGuid().ToString(format: "N");
        [JsonProperty(PropertyName = "recordCode")]
        public string RecordCode {get; set; } = "event";
        [JsonProperty(PropertyName = "recordId")]
        public string RecordId {get; set;}
        [JsonProperty(PropertyName = "eventName")]
        public string EventName {get; set;}
        [JsonProperty(PropertyName = "balance")]
        public decimal Balance {get; set;}    
    }
}