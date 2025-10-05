using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateMedicalExamDto

{
    public string Id { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Indication { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string PreparationRequired { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string DurationTime { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string DeliveryDeadline { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Description { get; set; } = "";
}
