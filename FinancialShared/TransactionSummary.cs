using System;
using System.Text.Json.Serialization;

namespace Financial.Shared;

public class TransactionSummary
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString(format: "D");
    [JsonPropertyName("recordId")]
    public string RecordId { get; set; }
    [JsonPropertyName("dateIncurred")]
    public DateTime DateIncurred { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
}
