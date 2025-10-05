using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Extensions;

static class AuthenticationExtension
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddJwtBearer("UserSchema", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["UserSecretKey"] ?? ""))
            };
        })
        .AddJwtBearer("AdminScheme", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["AdminSecretKey"] ?? ""))
            };
        });


        services.AddAuthorization(options =>
        {
            options.AddPolicy("UserPolicy", policy => policy.RequireAuthenticatedUser().AddAuthenticationSchemes("UserSchema"));
            options.AddPolicy("AdminPolicy", policy => policy.RequireAuthenticatedUser().AddAuthenticationSchemes("AdminScheme"));
        });

        return services;
    }
}