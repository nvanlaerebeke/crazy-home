using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Home.Api.Controllers.Request;

public sealed class AuthUpdateRequest {
    [JsonPropertyName("password")]
    [Required(AllowEmptyStrings = false)]
    public required string Password { get; set; }
}
