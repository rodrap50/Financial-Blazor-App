using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Financial.Shared;

public class FinancialEvent : EventBase
{
    [JsonPropertyName("transactions")]
    public List<TransactionEntry> Transactions { get; set; }
    [JsonPropertyName("transactionSummary")]
    public List<Transaction> TransactionSummary { get; set; }
}
