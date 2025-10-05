using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Helpers;
using WebApi.Services;
using WebApi.Extensions.ModelExtensions;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Bogus.DataSets;

namespace WebApi.Controllers.Private;

[ApiController]
[Route("api/v1/private/administradores")]
[Authorize(Policy = "AdminPolicy")]
public class PrivateAdministratorsController : ControllerBase
{
    private readonly IUserService<Administrator> _administratorService;
    private readonly ILogger<PrivateAdministratorsController> _logger;

    public PrivateAdministratorsController(IUserService<Administrator> administratorService, ILogger<PrivateAdministratorsController> logger)
    {
        _administratorService = administratorService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdministratorViewModel>>> GetAll()
    {
        try
        {
            var list = (await _administratorService.GetAllAsync())
                .Select(a => a.ToViewModel());

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
            var (users, totalCount) = await _administratorService.GetFilteredAsync(name, email, page, pageSize);

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
    public async Task<ActionResult<AdministratorViewModel>> GetById([FromRoute] string id)
    {
        try
        {
            var admin = await _administratorService.GetByIdAsync(id);

            if (admin == null)
                return StatusCode(404, ApiHelper.NotFound());

            return StatusCode(200, ApiHelper.Ok(admin.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateAdministratorDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var existingEmail = await _administratorService.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                return StatusCode(422, ApiHelper.UnprocessableEntity(Array.Empty<int>(), "Email está em uso"));

            var model = new Administrator
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = PasswordHelper.HashPassword(dto.Password)
            };

            await _administratorService.AddAsync(model);

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
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateAdministratorDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var user = await _administratorService.GetByIdAsync(id);
            if (user == null)
                return StatusCode(404, ApiHelper.NotFound());

            var existingEmail = await _administratorService.GetByEmailAsync(dto.Email, id);
            if (existingEmail != null)
                return StatusCode(422, ApiHelper.UnprocessableEntity(Array.Empty<int>(), "Email está em uso"));

            user.Name = dto.Name ?? user.Name;
            user.Email = dto.Email ?? user.Email;
            user.UpdatedAt = DateTime.UtcNow;

            await _administratorService.UpdateAsync(user);

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
            var admin = await _administratorService.GetByIdAsync(id);
            if (admin == null)
                return StatusCode(404, ApiHelper.NotFound());

            await _administratorService.DeleteAsync(admin);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}
