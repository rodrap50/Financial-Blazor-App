using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rodrap50.Financial.Api.Data.Responses
{
    public class ReserveNextAccountResponse {
        [JsonProperty(PropertyName = "accountId")]
        public string AccountId {get; set;}
    }
}