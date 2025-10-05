using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Helpers;
using WebApi.Services;
using WebApi.Extensions.ModelExtensions;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers.Private;

[ApiController]
[Route("api/v1/private/usuarios")]
[Authorize(Policy = "AdminPolicy")]
public class PrivateUsersController : ControllerBase
{
    private readonly IUserService<User> _userService;
    private readonly ILogger<PrivateUsersController> _logger;

    public PrivateUsersController(IUserService<User> userService, ILogger<PrivateUsersController> logger)
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

    [HttpGet("filtrados")]
    public async Task<ActionResult> GetFiltered(
        [FromQuery] string? name,
        [FromQuery] string? email,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var (users, totalCount) = await _userService.GetFilteredAsync(name, email, page, pageSize);

            var list = users.Select(u => u.ToViewModel());

            return StatusCode(200, ApiHelper.Ok(new
            {
                items = list,
                pagination = new
                {
                    page,
                    pageSize,
                    totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                }
            }));
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
            var model = await _userService.GetByIdAsync(id);

            if (model == null)
                return StatusCode(404, ApiHelper.NotFound());

            return StatusCode(200, ApiHelper.Ok(model.ToViewModel()));
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

            var model = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Cpf = dto.Cpf,
                Password = dto.Password
            };

            await _userService.AddAsync(model);

            var result = await GetById(model.Id);

            if (result.Result is ObjectResult objectResult)
            {
                objectResult.StatusCode = 201;
                return objectResult;
            }

            return StatusCode(201, result.Value);
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

            var model = await _userService.GetByIdAsync(id);
            if (model == null)
                return StatusCode(404, ApiHelper.NotFound());

            var existingEmail = await _userService.GetByEmailAsync(dto.Email, id);
            if (existingEmail != null)
                return StatusCode(422, ApiHelper.UnprocessableEntity(Array.Empty<int>(), "Email está em uso"));

            model.Name = dto.Name ?? model.Name;
            model.Email = dto.Email ?? model.Email;
            model.UpdatedAt = DateTime.UtcNow;

            await _userService.UpdateAsync(model);

            return StatusCode(200, ApiHelper.Ok());
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
