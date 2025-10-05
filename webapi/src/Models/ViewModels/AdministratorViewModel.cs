namespace WebApi.Models.ViewModels;

public class AdministratorViewModel
{
    public string Id { get; set; } = String.Empty;

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}