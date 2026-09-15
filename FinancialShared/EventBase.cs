using System;
using System.Text.Json.Serialization;

namespace Financial.Shared;

public class EventBase
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString(format: "D");
    [JsonPropertyName("recordCode")]
    public string RecordCode { get; set; } = "event";
    [JsonPropertyName("recordId")]
    public string RecordId { get; set; }
    [JsonPropertyName("eventName")]
    public string EventName { get; set; }
    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }
}
