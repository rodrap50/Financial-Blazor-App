using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FinancialApp.Data
{
    public class AccountEntry : IIDEntry {
         [JsonProperty(PropertyName = "id")]
        public string Id {get; set; } = Guid.NewGuid().ToString(format: "D");
       
        [JsonProperty(PropertyName = "accountName")]
        public string AccountName {get; set;}  

        public string GetId(){

            return Id;

        }

        public string GetName(){

            return AccountName;
        }


    }
}