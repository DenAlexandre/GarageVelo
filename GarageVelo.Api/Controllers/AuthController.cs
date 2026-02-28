using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GarageVelo.Api.Data;
using GarageVelo.Api.DTOs;
using GarageVelo.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GarageVelo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email.ToLower());
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Email ou mot de passe incorrect." });

        var token = GenerateJwtToken(user);
        await SaveLoginSession(user.Id, token);

        return Ok(new LoginResponse
        {
            Token = token,
            User = MapUserDto(user)
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email.ToLower()))
            return Conflict(new { message = "Cet email est déjà utilisé." });

        var user = new UserEntity
        {
            Email = request.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        await SaveLoginSession(user.Id, token);

        return CreatedAtAction(nameof(Login), new LoginResponse
        {
            Token = token,
            User = MapUserDto(user)
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        var session = await _db.LoginSessions.FirstOrDefaultAsync(s => s.Token == token && !s.IsRevoked);
        if (session != null)
        {
            session.IsRevoked = true;
            await _db.SaveChangesAsync();
        }

        return Ok(new { message = "Déconnexion réussie." });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _db.Users.FindAsync(userId);
        if (user == null)
            return NotFound();

        return Ok(MapUserDto(user));
    }

    private string GenerateJwtToken(UserEntity user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(7);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task SaveLoginSession(Guid userId, string token)
    {
        var session = new LoginSessionEntity
        {
            UserId = userId,
            Token = token,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            UserAgent = HttpContext.Request.Headers.UserAgent.ToString(),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _db.LoginSessions.Add(session);
        await _db.SaveChangesAsync();
    }

    private static UserDto MapUserDto(UserEntity user) => new()
    {
        Id = user.Id.ToString(),
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName
    };
}
