using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class RegisterUserDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve ter 11 dígitos.")]
    public string Cpf { get; set; } = "";
}
