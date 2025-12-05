using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Home.Api.Objects.Theme;

public sealed class Theme {
    [JsonPropertyName("name")]
    [Required(AllowEmptyStrings = false)]
    public required string Name { get; init; }

    [JsonPropertyName("primary")]
    [Required(AllowEmptyStrings = false)]
    public required string Primary { get; init; }

    [JsonPropertyName("secondary")]
    [Required(AllowEmptyStrings = false)]
    public required string Secondary { get; init; }

    [JsonPropertyName("tertiary")]
    [Required(AllowEmptyStrings = false)]
    public required string Tertiary { get; init; }
}
