using System.Security.Cryptography;
using System.Text;
using WebApi.Models;

namespace WebApi.Services.Users;

public class UserService : IUserService
{
    public static readonly Dictionary<Guid, User> _users = new();
    public void CreateUser(User user)
    {
        _users.Add(user.Id, user);
    }

    public void DeleteUser(Guid id)
    {
        _users.Remove(id);
    }

    public User getUser(Guid id)
    {
        return _users[id];
    }

    public object getUsers()
    {
        return _users;
    }

    public void UpsertUser(User user)
    {
        _users[user.Id] = user;
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}