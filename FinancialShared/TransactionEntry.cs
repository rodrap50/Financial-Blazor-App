using System;
using System.Text.Json.Serialization;

namespace Financial.Shared;

public class TransactionEntry
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString(format: "D");

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
}
