using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateAppointmentPublicDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string ScheduleId { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Status { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Description { get; set; } = "";
}
