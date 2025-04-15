using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Financial.Api.Data.Base;

namespace Financial.Api.Data.Responses;

public class AccountsResponse {
    [JsonProperty(PropertyName = "id")]
    public string Id {get; set; }
    [JsonProperty(PropertyName = "recordCode")]
    public string RecordCode {get; set; } = "accountsummary";
    [JsonProperty(PropertyName = "recordId")]
    public string RecordId {get; set;}
    [JsonProperty(PropertyName = "balance")]
    public decimal Balance {get; set;}
    [JsonProperty(PropertyName = "nextAccountNumber")]
    public string NextAccountNumber {get; set;}
    [JsonProperty(PropertyName = "accountSummaries")]
    public AccountBase[] AccountSummaries {get; set;}
    [JsonProperty(PropertyName="nextEventNumber")]
    public string NextEventNumber {get; set;}
    [JsonProperty(PropertyName="eventSummaries")]
    public EventBase[] EventSummaries {get; set;}
}
