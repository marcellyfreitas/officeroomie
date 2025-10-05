namespace WebApi.Models.ViewModels;

public class AppointmentScheduleViewModel
{
    public string Id { get; set; } = String.Empty;

    public DateTime AppointmentDate { get; set; }

    public RoomViewModel? Room { get; set; } = null;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
