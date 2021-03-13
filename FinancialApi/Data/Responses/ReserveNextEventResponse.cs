using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rodrap50.Financial.Api.Data.Responses
{
    public class ReserveNextEventResponse {
        [JsonProperty(PropertyName = "eventId")]
        public string EventId {get; set;}
    }
}