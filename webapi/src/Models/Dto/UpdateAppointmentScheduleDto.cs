using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateAppointmentScheduleDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public DateTime InitialHour { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public DateTime FinalHour { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string RoomId { get; set; } = "";
}
