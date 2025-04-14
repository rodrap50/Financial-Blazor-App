using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinancialApp.Data
{
    public class GeneralAccountEntry : AccountEntry
    {
        [JsonProperty(PropertyName = "softAccountList")]
        public AccountEntry[] SoftAccountList { get; set; }

    }
}
