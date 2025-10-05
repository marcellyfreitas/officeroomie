using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateAppointmentRatingDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public int Rating { get; set; } = 0;

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Comment { get; set; } = String.Empty;

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string AppointmentId { get; set; } = String.Empty;

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string UserId { get; set; } = String.Empty;
}