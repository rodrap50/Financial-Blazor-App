using Newtonsoft.Json;

namespace Financial.Api.Data.Base;

public class GeneralAccountEntry : AccountEntry
{
    [JsonProperty(PropertyName = "softAccountList")]
    public AccountEntry[] SoftAccountList { get; set; }

}
