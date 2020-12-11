using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Rodrap50.Financial.Api.Data.Base;

namespace Rodrap50.Financial.Api.Data
{
    public class Account : AccountBase {

         [JsonProperty(PropertyName = "recordCode")]
        public string RecordCode {get; set; } = "account";
       
       [JsonProperty(PropertyName = "nextTransactionRecordId")]
        public string NextTransactionRecordId {get; set;} = "10000";
        
    }
}