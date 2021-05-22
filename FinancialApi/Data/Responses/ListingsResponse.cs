using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Rodrap50.Financial.Api.Data.Base;

namespace Rodrap50.Financial.Api.Data.Responses
{
    public class ListingsResponse
    {

        [JsonProperty(PropertyName = "generalAccountEntries")]
        public GeneralAccountEntry[] GeneralAccountEntries { get; set; }
        [JsonProperty(PropertyName = "eventEntries")]

        public EventEntry[] EventEntries { get; set; }
    }
}