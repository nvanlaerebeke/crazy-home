using System.Text.Json.Serialization;

namespace Home.Api.Objects.Mobile;

internal sealed class Theme {
    [JsonPropertyName("BackgroundImage")]
    public required string BackgroundImage { get; init; }
    [JsonPropertyName("BackgroundColor")]
    public required string BackgroundColor { get; init; }
}

