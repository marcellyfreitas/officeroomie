using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public class Convenio
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("name")]
    public string Name { get; set; } = "";

    [BsonElement("provider")]
    public string Provider { get; set; } = "";

    [BsonElement("healthPlanId")]
    public int HealthPlanId { get; set; }

    [BsonElement("startDate")]
    public DateTime StartDate { get; set; }

    [BsonElement("endDate")]
    public DateTime EndDate { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}