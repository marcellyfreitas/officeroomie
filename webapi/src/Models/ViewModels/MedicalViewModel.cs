namespace WebApi.Models.ViewModels;

public class MedicalViewModel
{
    public string Id { get; set; } = String.Empty;

    public string Name { get; set; } = String.Empty;

    public string Cpf { get; set; } = String.Empty;

    public string Email { get; set; } = String.Empty;

    public string Crm { get; set; } = String.Empty;

    public SpecializationViewModel? Specialization { get; set; } = null;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}