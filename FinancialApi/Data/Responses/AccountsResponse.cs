using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Rodrap50.Financial.Api.Data.Base;

namespace Rodrap50.Financial.Api.Data.Responses
{
    public class AccountsResponse {
        [JsonProperty(PropertyName = "id")]
        public string Id {get; set; }
        [JsonProperty(PropertyName = "recordCode")]
        public string RecordCode {get; set; } = "accountsummary";
        [JsonProperty(PropertyName = "recordId")]
        public string RecordId {get; set;}
        [JsonProperty(PropertyName = "balance")]
        public decimal Balance {get; set;}
        [JsonProperty(PropertyName = "nextAccountNumber")]
        public string NextAccountNumber {get; set;}
        [JsonProperty(PropertyName = "accountSummaries")]
        public AccountBase[] AccountSummaries {get; set;}
    }
}