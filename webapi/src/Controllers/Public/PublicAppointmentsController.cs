using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Helpers;
using WebApi.Services;
using WebApi.Extensions.ModelExtensions;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApi.Controllers.Public;

[ApiController]
[Route("api/v1/public/agendamentos")]
[Authorize(Policy = "UserPolicy")]
public class PublicAppointmentsController : ControllerBase
{
    private readonly IBaseService<Appointment> _appointmentService;
    private readonly IUserService<User> _userService;
    private readonly IMedicalService _medicalService;
    private readonly IBaseService<Schedule> _scheduleService;
    private readonly ILogger<PublicAppointmentsController> _logger;

    public PublicAppointmentsController
    (
        IBaseService<Appointment> appointmentService,
        IUserService<User> userService,
        IMedicalService medicalService,
        IBaseService<Schedule> scheduleService,
        ILogger<PublicAppointmentsController> logger
    )
    {
        _appointmentService = appointmentService;
        _userService = userService;
        _medicalService = medicalService;
        _scheduleService = scheduleService;
        _logger = logger;
    }

    protected async Task<User?> GetAuthenticatedUserAsync()
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return null;
            }

            var user = await _userService.GetByIdAsync(userIdClaim.ToString());

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("medicos/pesquisar")]
    public async Task<ActionResult> GetSearchOfMedical(
       [FromQuery] string? medicalName,
       [FromQuery] string? specializationId
    )
    {
        try
        {
            var list = await _medicalService.GetAllAsync();

            if (string.IsNullOrEmpty(medicalName) && string.IsNullOrEmpty(specializationId))
            {
                return StatusCode(200, ApiHelper.Ok(Array.Empty<int>()));
            }

            if (!string.IsNullOrEmpty(medicalName))
            {
                medicalName = medicalName.Trim();
                list = list.Where(m => m.Name != null && m.Name.Contains(medicalName!, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(specializationId))
            {
                specializationId = specializationId.Trim();
                list = list.Where(m => m.SpecializationId == specializationId);
            }


            var mylist = list.Select(m => m.ToViewModel());

            return StatusCode(200, ApiHelper.Ok(mylist));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpGet("horarios")]
    public async Task<ActionResult> GetAvailableSchedules(
        [FromQuery] string medicalId,
        [FromQuery] DateTime date
    )
    {
        try
        {
            if (string.IsNullOrEmpty(medicalId) || date == default)
                return StatusCode(422, ApiHelper.UnprocessableEntity("medicalId and date are required."));

            var schedules = await _scheduleService.GetAllAsync();

            schedules = schedules
                .Where(s =>
                    s.MedicalId == medicalId &&
                    s.InitialHour.Date == date.Date);

            return StatusCode(200, ApiHelper.Ok(schedules));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentViewModel>>> GetAllMy()
    {
        try
        {
            var user = await GetAuthenticatedUserAsync();

            if (user == null)
                return StatusCode(404, ApiHelper.NotFound());


            var list = await _appointmentService.GetAllAsync();

            var mylist = list.Where(a => a.UserId == user.Id).Select(a => a.ToViewModel());

            return StatusCode(200, ApiHelper.Ok(mylist));
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
            var Appointment = await _appointmentService.GetByIdAsync(id);

            if (Appointment == null)
                return StatusCode(404, ApiHelper.NotFound());

            return StatusCode(200, ApiHelper.Ok(Appointment.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateAppointmentPublicDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var user = await GetAuthenticatedUserAsync();

            if (user == null)
                return StatusCode(404, ApiHelper.NotFound());

            var model = new Appointment
            {
                UserId = user.Id,
                ScheduleId = dto.ScheduleId,
                Status = dto.Status,
                Description = dto.Description,
            };

            await _appointmentService.AddAsync(model);

            return StatusCode(201, ApiHelper.Ok(model.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateAppointmentPublicDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var model = await _appointmentService.GetByIdAsync(id);
            if (model == null)
                return StatusCode(404, ApiHelper.NotFound());

            model.ScheduleId = dto.ScheduleId;
            model.Description = dto.Description ?? model.Description;
            model.Status = dto.Status ?? model.Status;
            model.UpdatedAt = DateTime.UtcNow;

            await _appointmentService.UpdateAsync(model);

            return StatusCode(200, ApiHelper.Ok(model.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

}
