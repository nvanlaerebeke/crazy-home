using System.Text.Json.Serialization;

namespace Home.Api.Objects.Audio;

public sealed class PlayList {
    [JsonPropertyName("collaborative")]
    public bool? Collaborative { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("externalUrls")]
    public Dictionary<string, string>? ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("public")]
    public bool? Public { get; init; }

    [JsonPropertyName("snapshotId")]
    public string? SnapshotId { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}
