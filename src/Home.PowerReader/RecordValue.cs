using System.Text.Json.Serialization;

namespace Home.PowerReader;

internal class RecordValue {
    [JsonPropertyName("unit")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required RecordUnit Unit { get; set; }

    [JsonPropertyName("value")]
    public required string Value { get; init; }
}
