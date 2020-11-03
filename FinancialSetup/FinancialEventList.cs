using Newtonsoft.Json;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;

namespace FinancialSetup
{
    public class FinancialEventList {
         [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Record {
            get { return "eventlist"; }
        }

        private List<FinancialEvent> _events;
        public List<FinancialEvent> Events {get { return _events; }}

        public FinancialEventList()
        {
            _events = new List<FinancialEvent>();
        }

    }
}
