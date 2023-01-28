namespace WebApi.Models;

public class User
{
    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Username { get; }
    public string Role { get; }
    public byte[] PasswordHash { get; }
    public byte[] PasswordSalt { get; }
    public User(
        Guid id,
        string firstName,
        string lastName,
        string username,
        string role,
        byte[] passwordHash,
        byte[] passwordSalt
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Role = role;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }
}