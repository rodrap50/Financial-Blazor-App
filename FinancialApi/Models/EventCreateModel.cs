using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Rodrap50.Financial.Api.Data;

namespace Rodrap50.Financial.Api.Models {
    public class EventCreateModel {
        public string EventName {get; set;}
   

        public FinancialEvent GenerateEvent() {
            FinancialEvent factory = new FinancialEvent();
            factory.EventName = this.EventName;
            factory.Balance = 0;
            return factory;
        }
    }
}