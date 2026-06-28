using Microsoft.AspNetCore.Authentication.Cookies;

namespace OfficeRoomie.Extensions;
public static class AuthorizeConfigExtension
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/authentication/login";
            options.LogoutPath = "/authentication/logout";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        });


        return services;
    }
}