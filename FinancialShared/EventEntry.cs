using System;
using System.Text.Json.Serialization;

namespace Financial.Shared;

public class EventEntry
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString(format: "D");

    [JsonPropertyName("eventName")]
    public string EventName { get; set; }
}
