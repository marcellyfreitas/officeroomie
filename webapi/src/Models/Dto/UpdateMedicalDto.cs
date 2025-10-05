using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateMedicalDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
    public string Name { get; set; } = String.Empty;

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; } = String.Empty;

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Crm { get; set; } = String.Empty;

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string SpecializationId { get; set; } = String.Empty;

}
