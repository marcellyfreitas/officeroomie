namespace WebApi.Models.ViewModels;

public class AppointmentRatingViewModel
{
    public string Id { get; set; } = String.Empty;

    public int Rating { get; set; } = 0;

    public string Comment { get; set; } = String.Empty;

    public AppointmentViewModel? Appointment { get; set; } = null;

    public UserViewModel? User { get; set; } = null;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}