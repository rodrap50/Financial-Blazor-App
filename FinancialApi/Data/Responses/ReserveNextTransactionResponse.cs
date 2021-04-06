using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rodrap50.Financial.Api.Data.Responses
{
    public class ReserveNextTransactionResponse {
        [JsonProperty(PropertyName = "transactionRecordId")]
        public string TransactionRecordId {get; set;}
    }
}