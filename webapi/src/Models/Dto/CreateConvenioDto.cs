using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateConvenioDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Provider { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public int HealthPlanId { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public DateTime EndDate { get; set; }
}
