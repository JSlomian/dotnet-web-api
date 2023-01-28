namespace WebApi.Contracts.User;

public record UpsertUserRequest(
    Guid id,
    string firstName,
    string lastName,
    string username,
    string role,
    string password
);