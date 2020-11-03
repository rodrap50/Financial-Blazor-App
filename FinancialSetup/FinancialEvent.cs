using Newtonsoft.Json;
using System.Runtime.Serialization;
using System;

namespace FinancialSetup
{
    public class FinancialEvent {
   [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        public string EventName { get; set;}
    }
}