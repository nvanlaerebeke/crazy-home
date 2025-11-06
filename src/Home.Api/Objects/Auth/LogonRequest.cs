using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Home.Api.Objects.Auth;

public sealed class LogonRequest {
    [JsonPropertyName("username")]
    [Required(AllowEmptyStrings = false)]
    public required string Username { get; set; }

    [JsonPropertyName("password")]
    [Required(AllowEmptyStrings = false)]
    public required string Password { get; set; }
}
