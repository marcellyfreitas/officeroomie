namespace WebApi.Models.ViewModels;

public class AppointmentViewModel
{
    public string Id { get; set; } = String.Empty;

    public string Description { get; set; } = "";

    public string Status { get; set; } = "";

    public UserViewModel? User { get; set; } = null;

    public ScheduleViewModel? Schedule { get; set; } = null;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
