using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models.ViewModels;

public class MedicalExamViewModel

{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = "";

    public string Indication { get; set; } = "";

    public string PreparationRequired { get; set; } = "";

    public string DurationTime { get; set; } = "";

    public string DeliveryDeadline { get; set; } = "";

    public string Description { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
