using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Financial.Api.Data;

namespace Financial.Api.Models {
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
