using System;
using Newtonsoft.Json;

namespace FinancialApp.Data
{
    public class GeneralAccountEntry : AccountEntry
    {
        [JsonProperty(PropertyName = "softAccountList")]
        public AccountEntry[] SoftAccountList { get; set; }

    }
}