using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public class Schedule
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("initialHour")]
    public DateTime InitialHour { get; set; }

    [BsonElement("finalHour")]
    public DateTime FinalHour { get; set; }

    [BsonElement("medicalId")]
    public string MedicalId { get; set; } = String.Empty;

    public Medical? Medical { get; set; } = null;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
