namespace WebApi.Contracts.User;

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Username,
    string Role,
    byte[] PasswordHash,
    byte[] PasswordSalt
);