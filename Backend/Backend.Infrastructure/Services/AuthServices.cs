using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Core.DTOs;
using Backend.Core.Exceptions;
using Backend.Core.Models;
using Backend.Core.Services;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Backend.Infrastructure.Services;

public class AuthServices : IAuthServices
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthServices(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = _context.Users.
            FirstOrDefault(x => x.Email == dto.Email.ToLower());
        if (existingUser is not null)
            throw new ValidationException("email", "An account with this email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow,
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return GenerateToken(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == dto.Email.ToLower());
        
        if ( user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new ValidationException("credentials", "Invalid email or password.");
        
        return GenerateToken(user);
    }


    private AuthResponseDto GenerateToken(User user)
    {
        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"];
        var expiry = int.Parse(_configuration["Jwt:ExpiryHours"]!);
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddHours(expiry);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: expiresAt,
            claims: claims,
            signingCredentials: credential
        );

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email,
            ExpiresAt = expiresAt
        };

    }
}