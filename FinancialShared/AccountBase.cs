using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Financial.Shared;

public class AccountBase
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString(format: "D");
    [JsonPropertyName("recordId")]
    public string RecordId { get; set; }

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; }

    [JsonPropertyName("softAccount")]
    public Boolean SoftAccount { get; set; }
    // General Account ID set when Soft Account is true
    [JsonPropertyName("generalAccountId")]
    public string GeneralAccountId { get; set; }
    // Soft Account List set when Soft Account is false
    [JsonPropertyName("softAccountList")]
    public List<AccountEntry> SoftAccountList { get; set; }
    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }
}
