using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Financial.Api.Data.Responses;

public class ReserveNextTransactionResponse {
    [JsonProperty(PropertyName = "transactionRecordId")]
    public string TransactionRecordId {get; set;}
}
