using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Home.Api.Objects.Auth;

public sealed class AuthRefresh {
    [JsonPropertyName("refresh_token")]
    [Required(AllowEmptyStrings = false)]
    public required string RefreshToken { get; set; }
}

