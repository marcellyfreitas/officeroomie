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
[Route("api/v1/private/especializacoes")]
[Authorize(Policy = "AdminPolicy")]
public class PrivateSpecializationsController : ControllerBase
{
    private readonly IBaseService<Specialization> _specializationService;
    private readonly ILogger<PrivateSpecializationsController> _logger;

    public PrivateSpecializationsController(IBaseService<Specialization> specializationService, ILogger<PrivateSpecializationsController> logger)
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
    public async Task<ActionResult<SpecializationViewModel>> GetById([FromRoute] string id)
    {
        try
        {
            var model = await _specializationService.GetByIdAsync(id);

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
    public async Task<IActionResult> Add([FromBody] CreateSpecializationDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));


            var model = new Specialization
            {
                Name = dto.Name,
                Description = dto.Description,
            };

            await _specializationService.AddAsync(model);

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
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateSpecializationDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var model = await _specializationService.GetByIdAsync(id);
            if (model == null)
                return StatusCode(404, ApiHelper.NotFound());

            model.Name = dto.Name ?? model.Name;
            model.Description = dto.Description ?? model.Description;

            await _specializationService.UpdateAsync(model);

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
            var model = await _specializationService.GetByIdAsync(id);
            if (model == null)
                return StatusCode(404, ApiHelper.NotFound());

            await _specializationService.DeleteAsync(model);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}
