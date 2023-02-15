using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using b_cars_backend.Configuration;
using b_cars_backend.Auth;
using b_cars_backend.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace b_cars_backend.Controllers;

[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly JwtConfig _jwtConfig;

    public AuthController(
        UserManager<User> userManager,
        IOptionsMonitor<JwtConfig> optionsMonitor)
    {
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistration user)
    {
        if (!ModelState.IsValid) return AuthResponse("Invalid Payload");
        var existingUser = await _userManager.FindByEmailAsync(user.Email);
        if (existingUser != null) return AuthResponse("Користувач з таким email вже зареєстрований");
        var newUser = new User
        {
            Email = user.Email,
            UserName = user.UserName,
            EmailConfirmed = true,
            PhoneNumber = user.Phone,
            PhoneNumberConfirmed = true
        };

        var isCreated = await _userManager.CreateAsync(newUser, user.Password);
        if (!isCreated.Succeeded)
        {
            return BadRequest(new RegistrationResponse
                { Errors = isCreated.Errors.Select(x => x.Description).ToList(), Success = false });
        }

        await _userManager.AddToRoleAsync(newUser, Constants.UserRole);


        var jwtToken = GenerateJwtToken(newUser);
        return Ok(new RegistrationResponse
        {
            Success = true,
            Token = jwtToken,
            UserName = newUser.UserName,
            Id = newUser.Id,
            PhoneNumber = newUser.PhoneNumber
        });
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
    {
        if (!ModelState.IsValid) return AuthResponse("Невірний логін");
        var existingUser = await _userManager.FindByEmailAsync(user.Email);
        if (existingUser == null) return AuthResponse("Логін не існує");
        var isValid = await _userManager.CheckPasswordAsync(existingUser, user.Password);
        if (!isValid) return AuthResponse("Невірний пароль");
        var jwtToken = GenerateJwtToken(existingUser);
        return Ok(new RegistrationResponse
        {
            Success = true,
            Token = jwtToken,
            UserName = existingUser.UserName,
            Id = existingUser.Id,
            PhoneNumber = existingUser.PhoneNumber
        });
    }

    private string GenerateJwtToken(IdentityUser<int> user)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
        var jwtDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = jwtHandler.CreateToken(jwtDescriptor);
        var jwtToken = jwtHandler.WriteToken(token);
        return jwtToken;
    }

    private BadRequestObjectResult AuthResponse(string message)
    {
        return BadRequest(new RegistrationResponse()
            { Errors = new List<string>() { message }, Success = false });
    }
}