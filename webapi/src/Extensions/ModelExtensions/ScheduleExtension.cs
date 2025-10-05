using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class ScheduleExtension
{
    public static ScheduleViewModel ToViewModel(this Schedule schedule)
    {
        return new ScheduleViewModel
        {
            Id = schedule.Id,
            InitialHour = schedule.InitialHour,
            FinalHour = schedule.FinalHour,
            Medical = schedule.Medical != null ? schedule.Medical.ToViewModel() : null,
            CreatedAt = schedule.CreatedAt,
            UpdatedAt = schedule.UpdatedAt,
        };
    }
}