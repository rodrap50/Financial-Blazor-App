using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Financial.Shared;

public class Account : AccountBase
{
    [JsonPropertyName("recordCode")]
    public string RecordCode { get; set; } = "account";

    [JsonPropertyName("nextTransactionRecordId")]
    public string NextTransactionRecordId { get; set; } = "10000";

    [JsonPropertyName("transactions")]
    public List<TransactionEntry> Transactions { get; set; }
    [JsonPropertyName("transactionSummary")]
    public List<Transaction> TransactionSummary { get; set; }
}
