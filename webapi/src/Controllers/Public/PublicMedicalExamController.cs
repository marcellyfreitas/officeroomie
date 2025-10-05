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
[Route("api/v1/public/exames")]
[Authorize(Policy = "UserPolicy")]
public class PublicMedicalExamController : ControllerBase
{
    private readonly IBaseService<MedicalExam> _medicalExamService;
    private readonly ILogger<PublicMedicalExamController> _logger;

    public PublicMedicalExamController(IBaseService<MedicalExam> medicalExamService, ILogger<PublicMedicalExamController> logger)
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
    public async Task<ActionResult<MedicalExamViewModel>> GetById([FromRoute] string id)
    {
        try
        {
            var medicalExam = await _medicalExamService.GetByIdAsync(id);

            if (medicalExam == null)
                return StatusCode(404, ApiHelper.NotFound());

            return StatusCode(200, ApiHelper.Ok(medicalExam.ToViewModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}
