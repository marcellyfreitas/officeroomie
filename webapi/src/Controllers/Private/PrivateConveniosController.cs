using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Helpers;
using WebApi.Services;
using WebApi.Models.ViewModels;
using WebApi.Extensions.ModelExtensions;

namespace WebApi.Controllers.Private;

[ApiController]
[Route("api/v1/private/convenios")]
[Authorize(Policy = "AdminPolicy")]
public class PrivateConveniosController : ControllerBase
{
    private readonly IBaseService<Convenio> _convenioService;
    private readonly ILogger<PrivateConveniosController> _logger;

    public PrivateConveniosController(IBaseService<Convenio> convenioService, ILogger<PrivateConveniosController> logger)
    {
        _convenioService = convenioService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConvenioViewModel>>> GetAll()
    {
        try
        {
            var list = (await _convenioService.GetAllAsync())
                .Select(c => c.ToViewModel());

            return StatusCode(200, ApiHelper.Ok(list));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConvenioViewModel>> GetById([FromRoute] string id)
    {
        try
        {
            var convenio = await _convenioService.GetByIdAsync(id);

            if (convenio == null)
                return StatusCode(404, ApiHelper.NotFound());

            return StatusCode(200, ApiHelper.Ok(convenio.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateConvenioDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var convenio = new Convenio
            {
                Name = dto.Name,
                Provider = dto.Provider,
                HealthPlanId = dto.HealthPlanId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _convenioService.AddAsync(convenio);

            var model = convenio.ToViewModel();

            return StatusCode(201, ApiHelper.Ok(model));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateConvenioDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var convenio = await _convenioService.GetByIdAsync(id);
            if (convenio == null)
                return StatusCode(404, ApiHelper.NotFound());

            if (!string.IsNullOrEmpty(dto.Name))
                convenio.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Provider))
                convenio.Provider = dto.Provider;

            if (dto.HealthPlanId.HasValue)
                convenio.HealthPlanId = dto.HealthPlanId.Value;

            if (dto.StartDate.HasValue)
                convenio.StartDate = dto.StartDate.Value;

            if (dto.EndDate.HasValue)
                convenio.EndDate = dto.EndDate.Value;

            convenio.UpdatedAt = DateTime.UtcNow;

            await _convenioService.UpdateAsync(convenio);

            var model = convenio.ToViewModel();

            return StatusCode(200, ApiHelper.Ok(model));
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
            var convenio = await _convenioService.GetByIdAsync(id);
            if (convenio == null)
                return StatusCode(404, ApiHelper.NotFound());

            await _convenioService.DeleteAsync(convenio);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}