namespace WebApi;

public class AuthUser
{
    public string Username { get; set; } = string.Empty;
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
}