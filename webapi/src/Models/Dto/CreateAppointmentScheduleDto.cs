using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateAppointmentScheduleDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public DateTime AppointmentDate { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string RoomId { get; set; } = "";
}
