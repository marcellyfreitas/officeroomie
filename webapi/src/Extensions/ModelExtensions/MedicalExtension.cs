using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class MedicalExtensions
{
    public static MedicalViewModel ToViewModel(this Medical medical)
    {
        return new MedicalViewModel()
        {
            Id = medical.Id,
            Name = medical.Name,
            Cpf = medical.Cpf,
            Email = medical.Email,
            Crm = medical.Crm,
            Specialization = medical.Specialization != null ? medical.Specialization!.ToViewModel() : null,
            CreatedAt = medical.CreatedAt,
            UpdatedAt = medical.UpdatedAt,
        };
    }
}