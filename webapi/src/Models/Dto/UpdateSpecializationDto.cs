using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateSpecializationDto
{
    public string Id { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Description { get; set; } = "";
}
