using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Financial.Api.Data.Base;

namespace Financial.Api.Data;

public class Account : AccountBase
{

    [JsonProperty(PropertyName = "recordCode")]
    public string RecordCode { get; set; } = "account";

    [JsonProperty(PropertyName = "nextTransactionRecordId")]
    public string NextTransactionRecordId { get; set; } = "10000";

    [JsonProperty(PropertyName = "transactions")]
    public List<TransactionEntry> Transactions { get; set; }
    [JsonProperty(PropertyName = "transactionSummary")]
    public List<Transaction> TransactionSummary { get; set; }

}
