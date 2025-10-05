using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Helpers;
using WebApi.Services;
using WebApi.Extensions.ModelExtensions;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers.Public;

[ApiController]
[Route("api/v1/public/usuarios")]
[Authorize(Policy = "UserPolicy")]
public class PublicUsersController : ControllerBase
{
    private readonly IUserService<User> _userService;
    private readonly ILogger<PublicUsersController> _logger;

    public PublicUsersController(IUserService<User> userService, ILogger<PublicUsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAll()
    {
        try
        {
            var list = (await _userService.GetAllAsync())
                .Select(u => u.ToViewModel());

            return StatusCode(200, ApiHelper.Ok(list));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserViewModel>> GetById([FromRoute] string id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return StatusCode(404, ApiHelper.NotFound());

            return StatusCode(200, ApiHelper.Ok(user.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserDto dto)
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

            await _userService.AddAsync(user);

            return StatusCode(201, ApiHelper.Ok(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return StatusCode(404, ApiHelper.NotFound());

            var existingEmail = await _userService.GetByEmailAsync(dto.Email, id);
            if (existingEmail != null)
                return StatusCode(422, ApiHelper.UnprocessableEntity(Array.Empty<int>(), "Email está em uso"));

            user.Name = dto.Name ?? user.Name;
            user.Email = dto.Email ?? user.Email;

            await _userService.UpdateAsync(user);

            return StatusCode(200, ApiHelper.Ok(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return StatusCode(404, ApiHelper.NotFound());

            await _userService.DeleteAsync(user);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}