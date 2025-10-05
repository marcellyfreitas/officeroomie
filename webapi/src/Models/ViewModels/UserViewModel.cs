namespace WebApi.Models.ViewModels;

public class UserViewModel
{
    public string Id { get; set; } = String.Empty;

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public string Cpf { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
