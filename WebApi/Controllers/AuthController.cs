using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Services.Users;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    public static AuthUser user = new AuthUser();

    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthController(
        IConfiguration configuration,
        IUserService userService
        )
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("/register")]
    public IActionResult RegisterUser(UserDto request)
    {
        _userService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.Username = request.Username;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        return Ok(user);
    }

    [HttpPost("/login")]
    public IActionResult Login(UserDto request)
    {
        if (user.Username != request.Username)
        {
            return BadRequest("User not found");
        }
        if (user.PasswordHash != null && user.PasswordSalt != null)
        {
            if (!_userService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong Password");

            }
        }
        string token = CreateToken(user);
        return Ok(token);
    }

    private string CreateToken(AuthUser user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred
            );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}