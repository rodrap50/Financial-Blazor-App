using System;
using System.Text.Json.Serialization;

namespace Financial.Shared;

public class AccountEntry
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString(format: "D");

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; }
}
