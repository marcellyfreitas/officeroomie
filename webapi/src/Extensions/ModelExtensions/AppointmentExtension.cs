using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class AppointmentExtension
{
    public static AppointmentViewModel ToViewModel(this Appointment model)
    {
        return new AppointmentViewModel
        {
            Id = model.Id,
            User = model.User != null ? model.User!.ToViewModel() : null,
            Schedule = model.Schedule != null ? model.Schedule!.ToViewModel() : null,
            Description = model.Description,
            Status = model.Status,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
        };
    }
}