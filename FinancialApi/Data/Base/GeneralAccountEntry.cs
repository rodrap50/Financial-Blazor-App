using Newtonsoft.Json;

namespace Rodrap50.Financial.Api.Data.Base
{
    public class GeneralAccountEntry : AccountEntry
    {
        [JsonProperty(PropertyName = "softAccountList")]
        public AccountEntry[] SoftAccountList { get; set; }

    }
}