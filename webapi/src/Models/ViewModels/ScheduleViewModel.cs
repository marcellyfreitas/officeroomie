namespace WebApi.Models.ViewModels;

public class ScheduleViewModel
{
    public string Id { get; set; } = String.Empty;

    public DateTime InitialHour { get; set; }

    public DateTime FinalHour { get; set; }

    public MedicalViewModel? Medical { get; set; } = null;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
