using System.Text.Json.Serialization;

namespace Home.Api.Objects.Mqtt;

internal sealed class JoinDuration {
    [JsonPropertyName("total_seconds")]
    public double TotalSeconds { get; init; }
}

