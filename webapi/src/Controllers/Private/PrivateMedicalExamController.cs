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
[Route("api/v1/private/exames")]
[Authorize(Policy = "AdminPolicy")]
public class PrivateMedicalExamsController : ControllerBase
{
    private readonly IBaseService<MedicalExam> _medicalExamService;
    private readonly ILogger<PrivateMedicalExamsController> _logger;

    public PrivateMedicalExamsController(IBaseService<MedicalExam> medicalExamService, ILogger<PrivateMedicalExamsController> logger)
    {
        _medicalExamService = medicalExamService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalExamViewModel>>> GetAll()
    {
        try
        {
            var list = (await _medicalExamService.GetAllAsync())
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
    public async Task<ActionResult<MedicalExamViewModel>> GetById([FromRoute] string id)
    {
        try
        {
            var MedicalExam = await _medicalExamService.GetByIdAsync(id);

            if (MedicalExam == null)
                return StatusCode(404, ApiHelper.NotFound());

            return StatusCode(200, ApiHelper.Ok(MedicalExam.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateMedicalExamDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));


            var medicalExam = new MedicalExam
            {
                Name = dto.Name,
                Indication = dto.Indication,
                PreparationRequired = dto.PreparationRequired,
                DurationTime = dto.DurationTime,
                DeliveryDeadline = dto.DeliveryDeadline,
                Description = dto.Description,
            };

            await _medicalExamService.AddAsync(medicalExam);

            var model = medicalExam.ToViewModel();

            return StatusCode(201, ApiHelper.Ok(model));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateMedicalExamDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));

            var medicalExam = await _medicalExamService.GetByIdAsync(id);
            if (medicalExam == null)
                return StatusCode(404, ApiHelper.NotFound());

            medicalExam.Name = dto.Name ?? medicalExam.Name;
            medicalExam.Description = dto.Description ?? medicalExam.Description;

            await _medicalExamService.UpdateAsync(medicalExam);

            var model = medicalExam.ToViewModel();

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
            var medicalExam = await _medicalExamService.GetByIdAsync(id);
            if (medicalExam == null)
                return StatusCode(404, ApiHelper.NotFound());

            await _medicalExamService.DeleteAsync(medicalExam);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}
