using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Financial.Api.Data.Base;

public class AccountBase
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = Guid.NewGuid().ToString(format: "D");
    [JsonProperty(PropertyName = "recordId")]
    public string RecordId { get; set; }

    [JsonProperty(PropertyName = "accountName")]
    public string AccountName { get; set; }

    [JsonProperty(PropertyName = "softAccount")]
    public Boolean SoftAccount { get; set; }
    // General Account ID set when Soft Account is true
    [JsonProperty(PropertyName = "generalAccountId")]
    public string GeneralAccountId { get; set; }
    // Soft Account List set when Soft Account is false
    [JsonProperty(PropertyName = "softAccountList")]
    public List<AccountEntry> SoftAccountList { get; set; }
    [JsonProperty(PropertyName = "balance")]
    public decimal Balance { get; set; }
}
