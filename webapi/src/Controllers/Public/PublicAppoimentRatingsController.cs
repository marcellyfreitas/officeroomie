using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions.ModelExtensions;
using WebApi.Helpers;
using WebApi.Models.Dto;
using WebApi.Models;
using WebApi.Models.ViewModels;
using WebApi.Services;

namespace WebApi.Controllers.Public;

[ApiController]
[Route("api/v1/public/agendamento-avaliacoes")]
[Authorize(Policy = "UserPolicy")]
public class PublicAppointmentRatingsController : ControllerBase
{
    private readonly IBaseService<AppointmentRating> _AppointmentRatingService;
    private readonly ILogger<PublicAppointmentRatingsController> _logger;

    public PublicAppointmentRatingsController(IBaseService<AppointmentRating> AppointmentRatingService, ILogger<PublicAppointmentRatingsController> logger)
    {
        _AppointmentRatingService = AppointmentRatingService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentRatingViewModel>>> GetAll()
    {
        try
        {
            var ratings = (await _AppointmentRatingService.GetAllAsync())
                .Select(a => a.ToViewModel());
            return StatusCode(200, ApiHelper.Ok(ratings));

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
            var rating = await _AppointmentRatingService.GetByIdAsync(id);

            if (rating == null)
                return StatusCode(404, ApiHelper.NotFound());
            return StatusCode(200, ApiHelper.Ok(rating.ToViewModel()));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            return StatusCode(500, ApiHelper.InternalServerError());

        }

    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateAppointmentRatingDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
            {
                return StatusCode(400, ApiHelper.UnprocessableEntity());
            }


            var rating = new AppointmentRating()
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                AppointmentId = dto.AppointmentId,
                UserId = dto.UserId,
                CreatedAt = DateTime.Now,
            };

            await _AppointmentRatingService.AddAsync(rating);

            var model = rating.ToViewModel();

            return StatusCode(201, ApiHelper.Ok(model));
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
            var rating = await _AppointmentRatingService.GetByIdAsync(id);
            if (rating == null)
                return StatusCode(404, ApiHelper.NotFound());

            await _AppointmentRatingService.DeleteAsync(rating);
            return StatusCode(202, ApiHelper.Ok("Solicitação aceita"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] string id, [FromBody] UpdateAppointmentRatingDto dto)
    {
        try
        {
            if (!TryValidateModel(dto))
                return StatusCode(422, ApiHelper.UnprocessableEntity());

            var rating = await _AppointmentRatingService.GetByIdAsync(id);
            if (rating == null)
                return StatusCode(404, ApiHelper.NotFound());

            rating.Rating = dto.Rating;
            rating.Comment = dto.Comment;
            rating.AppointmentId = dto.AppointmentId;
            rating.UserId = dto.UserId;
            rating.UpdatedAt = DateTime.Now;

            await _AppointmentRatingService.UpdateAsync(rating);
            var model = rating.ToViewModel();
            return StatusCode(201, ApiHelper.Ok(model));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

}