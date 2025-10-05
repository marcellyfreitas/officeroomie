using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions.ModelExtensions;
using WebApi.Helpers;
using WebApi.Models.Dto;
using WebApi.Models;
using WebApi.Models.ViewModels;
using WebApi.Services;

namespace WebApi.Controllers.Private;

[ApiController]
[Route("api/v1/private/medicos")]
[Authorize(Policy = "AdminPolicy")]
public class PrivateMedicalController : ControllerBase
{
    private readonly IMedicalService _medicalService;
    private readonly ILogger<PrivateMedicalController> _logger;

    public PrivateMedicalController(IMedicalService medicalService, ILogger<PrivateMedicalController> logger)
    {
        _medicalService = medicalService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalViewModel>>> GetAll()
    {
        try
        {
            var medicals = (await _medicalService.GetAllAsync())
                .Select(a => a.ToViewModel());
            return StatusCode(200, ApiHelper.Ok(medicals));

        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalViewModel>> GetById([FromRoute] string id)
    {
        try
        {
            var admin = await _medicalService.GetByIdAsync(id);

            if (admin == null)
                return StatusCode(404, ApiHelper.NotFound());
            return StatusCode(200, ApiHelper.Ok(admin.ToViewModel()));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            return StatusCode(500, ApiHelper.InternalServerError());

        }

    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateMedicalDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));


            var existingEmail = await _medicalService.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                return StatusCode(422, ApiHelper.UnprocessableEntity("Email já existe!"));

            var model = new Medical()
            {
                Name = dto.Name,
                Email = dto.Email,
                Crm = dto.Crm,
                Cpf = dto.Cpf,
                SpecializationId = dto.SpecializationId,
            };

            await _medicalService.AddAsync(model);

            var result = await GetById(model.Id);

            if (result.Result is ObjectResult objectResult)
            {
                objectResult.StatusCode = 201;
                return objectResult;
            }

            return StatusCode(201, result.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        try
        {
            var model = await _medicalService.GetByIdAsync(id);
            if (model == null)
                return StatusCode(404, ApiHelper.NotFound());

            await _medicalService.DeleteAsync(model);
            return StatusCode(202, ApiHelper.Ok());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] string id, [FromBody] UpdateMedicalDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity());

            var user = await _medicalService.GetByIdAsync(id);
            if (user == null)
                return StatusCode(404, ApiHelper.NotFound());

            var existingEmail = await _medicalService.GetByEmailAsync(dto.Email, id);
            if (existingEmail != null)
                return StatusCode(422, ApiHelper.UnprocessableEntity(Array.Empty<int>(), "Email está em uso"));


            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Crm = dto.Crm;
            user.SpecializationId = dto.SpecializationId;
            user.UpdatedAt = DateTime.Now;

            await _medicalService.UpdateAsync(user);

            return StatusCode(201, ApiHelper.Ok());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

}