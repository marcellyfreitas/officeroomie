using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Dto;
using WebApi.Helpers;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Extensions.ModelExtensions;
using WebApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IUserService<User> _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthenticationService authService, IUserService<User> userService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("user/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var token = await _authService.LoginUserAsync(dto);
            if (token == null)
                return StatusCode(401, ApiHelper.Unauthorized("Credenciais inválidas"));

            return StatusCode(200, ApiHelper.Ok(new { token }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost("admin/login")]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var token = await _authService.LoginAdminAsync(dto);
            if (token == null)
                return StatusCode(401, ApiHelper.Unauthorized("Credenciais inválidas"));

            return StatusCode(200, ApiHelper.Ok(new { token }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost("user/register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var existingEmail = await _userService.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                return StatusCode(422, ApiHelper.UnprocessableEntity(Array.Empty<int>(), "Email está em uso"));

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Cpf = dto.Cpf,
                Password = dto.Password,
            };


            var token = await _authService.RegisterUserAsync(user);

            if (token.IsNullOrEmpty())
            {
                return StatusCode(401, ApiHelper.Unauthorized("Credenciais inválidas"));
            }

            return StatusCode(200, ApiHelper.Ok(new { token }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpGet("user")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> GetUserAsync()
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c =>
                c.Type == JwtRegisteredClaimNames.Sub ||
                c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(401, ApiHelper.Unauthorized("Token inválido"));
            }

            var user = await _authService.GetUserAsync(userId);

            if (user == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            var model = user.ToViewModel();

            return StatusCode(200, ApiHelper.Ok(model));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao carregar dados de usuário");
            throw;
        }
    }

    [HttpGet("admin")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetAdminAsync()
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c =>
                c.Type == JwtRegisteredClaimNames.Sub ||
                c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(401, ApiHelper.Unauthorized("Token inválido"));
            }

            var user = await _authService.GetAdminAsync(userId);

            if (user == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            var model = user.ToViewModel();

            return StatusCode(200, ApiHelper.Ok(model));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao carregar dados de usuário");
            throw;
        }
    }

}
