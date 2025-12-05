namespace Home.Auth.Objects;

public sealed class CreateUserDto {
    public required string  UserName { get; set; }
    public required string Password { get; set; }
}

