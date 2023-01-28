using WebApi.Contracts.User;
using WebApi.Models;

namespace WebApi.Services.Users;

public interface IUserService
{
    void CreateUser(User user);
    void DeleteUser(Guid id);
    User getUser(Guid id);
    object getUsers();
    void UpsertUser(User user);
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}