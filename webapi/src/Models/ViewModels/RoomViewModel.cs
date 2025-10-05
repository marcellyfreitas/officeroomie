namespace WebApi.Models.ViewModels;

public class RoomViewModel
{
    public string Id { get; set; } = String.Empty;

    public string Name { get; set; } = "";

    public int Capacity { get; set; } = 0;

    public string Status { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
