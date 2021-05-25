using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FinancialApp.Data;

namespace FinancialApp.Data
{
    public class ListingsResponse
    {

        [JsonProperty(PropertyName = "generalAccountEntries")]
        public GeneralAccountEntry[] GeneralAccountEntries { get; set; }
        [JsonProperty(PropertyName = "eventEntries")]

        public EventEntry[] EventEntries { get; set; }
    }
}