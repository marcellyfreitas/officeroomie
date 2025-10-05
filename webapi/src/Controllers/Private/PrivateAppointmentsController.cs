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
[Route("api/v1/private/agendamentos")]
[Authorize(Policy = "AdminPolicy")]
public class PrivateAppointmentsController : ControllerBase
{
    private readonly IBaseService<Appointment> _appointmentService;
    private readonly ILogger<PrivateAppointmentsController> _logger;

    public PrivateAppointmentsController(IBaseService<Appointment> appointmentService, ILogger<PrivateAppointmentsController> logger)
    {
        _appointmentService = appointmentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentViewModel>>> GetAll()
    {
        try
        {
            var list = (await _appointmentService.GetAllAsync())
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
    public async Task<ActionResult<AppointmentViewModel>> GetById([FromRoute] string id)
    {
        try
        {
            var model = await _appointmentService.GetByIdAsync(id);

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
    public async Task<IActionResult> Add([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var model = new Appointment
            {
                UserId = dto.UserId,
                ScheduleId = dto.ScheduleId,
                Status = dto.Status,
                Description = dto.Description,
            };

            await _appointmentService.AddAsync(model);

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
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateAppointmentDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var model = await _appointmentService.GetByIdAsync(id);
            if (model == null)
                return StatusCode(404, ApiHelper.NotFound());

            model.UserId = dto.UserId;
            model.ScheduleId = dto.ScheduleId;
            model.Description = dto.Description ?? model.Description;
            model.Status = dto.Status ?? model.Status;
            model.UpdatedAt = DateTime.UtcNow;

            await _appointmentService.UpdateAsync(model);

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
            var model = await _appointmentService.GetByIdAsync(id);
            if (model == null)
                return StatusCode(404, ApiHelper.NotFound());

            await _appointmentService.DeleteAsync(model);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}
