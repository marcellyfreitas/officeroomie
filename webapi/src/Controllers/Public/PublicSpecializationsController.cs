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
[Route("api/v1/public/especializacoes")]
[Authorize(Policy = "UserPolicy")]
public class PublicSpecializationsController : ControllerBase
{
    private readonly IBaseService<Specialization> _specializationService;
    private readonly ILogger<PublicSpecializationsController> _logger;

    public PublicSpecializationsController(IBaseService<Specialization> specializationService, ILogger<PublicSpecializationsController> logger)
    {
        _specializationService = specializationService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SpecializationViewModel>>> GetAll()
    {
        try
        {
            var list = (await _specializationService.GetAllAsync())
                .Select(s => s.ToViewModel());

            return StatusCode(200, ApiHelper.Ok(list));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SpecializationViewModel>> GetById([FromRoute] string id)
    {
        try
        {
            var specialization = await _specializationService.GetByIdAsync(id);

            if (specialization == null)
                return StatusCode(404, ApiHelper.NotFound());

            return StatusCode(200, ApiHelper.Ok(specialization.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateSpecializationDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var specialization = new Specialization
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _specializationService.AddAsync(specialization);

            return StatusCode(201, ApiHelper.Ok(specialization.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateSpecializationDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var specialization = await _specializationService.GetByIdAsync(id);
            if (specialization == null)
                return StatusCode(404, ApiHelper.NotFound());

            specialization.Name = dto.Name ?? specialization.Name;
            specialization.Description = dto.Description ?? specialization.Description;

            await _specializationService.UpdateAsync(specialization);

            return StatusCode(200, ApiHelper.Ok(specialization.ToViewModel()));
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
            var specialization = await _specializationService.GetByIdAsync(id);
            if (specialization == null)
                return StatusCode(404, ApiHelper.NotFound());

            await _specializationService.DeleteAsync(specialization);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}
