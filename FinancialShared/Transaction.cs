using System;
using System.Text.Json.Serialization;

namespace Financial.Shared;

public class Transaction : TransactionBase
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString(format: "D");
    [JsonPropertyName("recordCode")]
    public string RecordCode { get; set; } = "transaction";
}
