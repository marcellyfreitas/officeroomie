using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Database;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Models.Dto;

namespace WebApi.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthenticationService(IApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string?> LoginUserAsync(LoginDto dto)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, dto.Email);
        var user = await _context.Users.Find(filter).FirstOrDefaultAsync();

        if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.Password))
            return null;

        return GenerateJwtToken(user.Id, user.Email, user.Name, "UserSchema", "UserSecretKey");
    }

    public async Task<string?> LoginAdminAsync(LoginDto dto)
    {
        var filter = Builders<Administrator>.Filter.Eq(a => a.Email, dto.Email);
        var admin = await _context.Administrators.Find(filter).FirstOrDefaultAsync();

        if (admin == null || !PasswordHelper.VerifyPassword(dto.Password, admin.Password))
            return null;

        return GenerateJwtToken(admin.Id, admin.Email, admin.Name, "AdminScheme", "AdminSecretKey");
    }

    private string GenerateJwtToken(string id, string email, string name, string scheme, string secretKeyName)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings[secretKeyName] ?? "");
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("name", name),
            new Claim("scheme", scheme)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User?> GetUserAsync(string id)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        return await _context.Users.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Administrator?> GetAdminAsync(string id)
    {
        var filter = Builders<Administrator>.Filter.Eq(u => u.Id, id);
        return await _context.Administrators.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<string> RegisterUserAsync(User user)
    {
        user.Password = PasswordHelper.HashPassword(user.Password);

        await _context.Users.InsertOneAsync(user);

        var token = GenerateJwtToken(user.Id, user.Email, user.Name, "UserSchema", "UserSecretKey");

        return token;
    }
}
