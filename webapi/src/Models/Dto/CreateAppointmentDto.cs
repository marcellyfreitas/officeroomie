using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateAppointmentDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string UserId { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string ScheduleId { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Status { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Description { get; set; } = "";
}
