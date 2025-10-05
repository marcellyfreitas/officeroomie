using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class ConvenioExtension
{
    public static ConvenioViewModel ToViewModel(this Convenio convenio)
    {
        return new ConvenioViewModel
        {
            Id = convenio.Id,
            Name = convenio.Name,
            Provider = convenio.Provider,
            HealthPlanId = convenio.HealthPlanId,
            StartDate = convenio.StartDate,
            EndDate = convenio.EndDate,
            CreatedAt = convenio.CreatedAt,
            UpdatedAt = convenio.UpdatedAt,
        };
    }
}