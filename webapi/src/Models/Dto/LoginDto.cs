using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class LoginDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    public string Password { get; set; } = "";
}
