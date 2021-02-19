using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Rodrap50.Financial.Api.Data.Base;

namespace Rodrap50.Financial.Api.Data.Responses
{
    public class AccountResponse : AccountBase {


        [JsonProperty(PropertyName = "nextTransactionRecordId")]
        public string NextTransactioRecordId {get; set; }

    }
}