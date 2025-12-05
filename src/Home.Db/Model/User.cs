using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Db.Model;

[Table("user")]
public sealed class User : Entity {
    [Column("username")]
    public required string UserName { get; set; }

    [Column("password_hash")]
    public required string PasswordHash { get; set; }

    [Column("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;

    [Column("refresh_token_expiry")]
    public required DateTime? RefreshTokenExpiry { get; set; }
}
