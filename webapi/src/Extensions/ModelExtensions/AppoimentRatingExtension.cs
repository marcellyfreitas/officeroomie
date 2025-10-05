using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class AppointmentRatingExtension
{
    public static AppointmentRatingViewModel ToViewModel(this AppointmentRating appointmentRating)
    {
        return new AppointmentRatingViewModel()
        {
            Id = appointmentRating.Id,
            Rating = appointmentRating.Rating,
            Comment = appointmentRating.Comment,
            Appointment = appointmentRating.Appointment != null ? appointmentRating.Appointment!.ToViewModel() : null,
            User = appointmentRating.User != null ? appointmentRating.User!.ToViewModel() : null,
            CreatedAt = appointmentRating.CreatedAt,
            UpdatedAt = appointmentRating.UpdatedAt,
        };
    }
}