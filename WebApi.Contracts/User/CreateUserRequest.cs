namespace WebApi.Contracts.User;

public record CreateUserRequest(
        string firstName,
        string lastName,
        string username,
        string role,
        string password
);