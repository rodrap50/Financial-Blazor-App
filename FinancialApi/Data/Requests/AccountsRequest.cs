using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Financial.Shared;

namespace Financial.Api.Data.Requests;

public class AccountsRequest {
    public Account[] accounts {get; set;}
}
