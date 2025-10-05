using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Dto;

namespace WebApi.Services;

public interface IAuthenticationService
{
    Task<string?> LoginUserAsync(LoginDto dto);

    Task<string?> LoginAdminAsync(LoginDto dto);

    Task<User?> GetUserAsync(string id);

    Task<Administrator?> GetAdminAsync(string id);

    Task<string> RegisterUserAsync(User user);

}