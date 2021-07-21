using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Rodrap50.Financial.Api.Data.Base;

namespace Rodrap50.Financial.Api.Data.Requests
{
    public class AccountsRequest {
        public Account[] accounts {get; set;}
    }
}