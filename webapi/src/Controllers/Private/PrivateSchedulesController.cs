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
[Route("api/v1/private/horarios")]
[Authorize(Policy = "AdminPolicy")]
public class PrivateSchedulesController : ControllerBase
{
    private readonly IBaseService<Schedule> _scheduleService;
    private readonly ILogger<PrivateSchedulesController> _logger;

    public PrivateSchedulesController(IBaseService<Schedule> scheduleService, ILogger<PrivateSchedulesController> logger)
    {
        _scheduleService = scheduleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleViewModel>>> GetAll()
    {
        try
        {
            var list = (await _scheduleService.GetAllAsync())
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
    public async Task<ActionResult<ScheduleViewModel>> GetById([FromRoute] string id)
    {
        try
        {
            var model = await _scheduleService.GetByIdAsync(id);

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
    public async Task<IActionResult> Add([FromBody] CreateScheduleDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));


            if (dto.FinalHour <= dto.InitialHour)
            {
                ModelState.AddModelError("FinalHour", "FinalHour deve ser maior que InitialHour.");
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var model = new Schedule
            {
                InitialHour = dto.InitialHour.ToLocalTime(),
                FinalHour = dto.FinalHour.ToLocalTime(),
                MedicalId = dto.MedicalId,
            };

            await _scheduleService.AddAsync(model);

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
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateScheduleDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var model = await _scheduleService.GetByIdAsync(id);
            if (model == null)
                return StatusCode(404, ApiHelper.NotFound());

            model.InitialHour = dto.InitialHour.ToLocalTime();
            model.FinalHour = dto.FinalHour.ToLocalTime();
            model.MedicalId = dto.MedicalId;
            model.UpdatedAt = DateTime.UtcNow;

            await _scheduleService.UpdateAsync(model);

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
            var model = await _scheduleService.GetByIdAsync(id);
            if (model == null)
                return StatusCode(404, ApiHelper.NotFound());

            await _scheduleService.DeleteAsync(model);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}
