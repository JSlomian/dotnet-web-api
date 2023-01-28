using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.User;
using WebApi.Models;
using WebApi.Services.Users;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userService.getUsers();
        return Ok(users);
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserRequest request)
    {
        _userService.CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);
        var user = new User(
            Guid.NewGuid(),
            request.firstName,
            request.lastName,
            request.username,
            request.role,
            passwordHash,
            passwordSalt
        );

        _userService.CreateUser(user);

        var response = new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Username,
            user.Role,
            user.PasswordHash,
            user.PasswordSalt
        );

        return CreatedAtAction(
            actionName: nameof(GetUser),
            routeValues: new { id = user.Id },
            value: response
            );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetUser(Guid id)
    {
        User user = _userService.getUser(id);
        var response = new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Username,
            user.Role,
            user.PasswordHash,
            user.PasswordSalt
        );
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpserUser(Guid id, UpsertUserRequest request)
    {
        _userService.CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);
        var user = new User(
            id,
            request.firstName,
            request.lastName,
            request.username,
            request.role,
            passwordHash,
            passwordSalt
        );
        _userService.UpsertUser(user);
        // return 201 if new user was created

        return Ok(request);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteUser(Guid id)
    {
        _userService.DeleteUser(id);
        return NoContent();
    }
}