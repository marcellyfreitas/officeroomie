using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class MedicalExamExtension
{
    public static MedicalExamViewModel ToViewModel(this MedicalExam medicalExam)
    {
        return new MedicalExamViewModel
        {
            Id = medicalExam.Id,
            Name = medicalExam.Name,
            Indication = medicalExam.Indication,
            PreparationRequired = medicalExam.PreparationRequired,
            DurationTime = medicalExam.DurationTime,
            DeliveryDeadline = medicalExam.DeliveryDeadline,
            Description = medicalExam.Description,
            CreatedAt = medicalExam.CreatedAt,
            UpdatedAt = medicalExam.UpdatedAt,
        };
    }
}