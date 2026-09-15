using System.Text.Json.Serialization;

namespace Financial.Shared;

public class GeneralAccountEntry : AccountEntry
{
    [JsonPropertyName("softAccountList")]
    public AccountEntry[] SoftAccountList { get; set; }
}
