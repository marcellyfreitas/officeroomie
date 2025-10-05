using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class SpecializationExtension
{
    public static SpecializationViewModel ToViewModel(this Specialization specialization)
    {
        return new SpecializationViewModel
        {
            Id = specialization.Id,
            Name = specialization.Name,
            Description = specialization.Description,
            CreatedAt = specialization.CreatedAt,
            UpdatedAt = specialization.UpdatedAt,
        };
    }
}