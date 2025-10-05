using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public class MedicalExam

{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("name")]
    public string Name { get; set; } = "";

    [BsonElement("indication")]
    public string Indication { get; set; } = "";

    [BsonElement("preparationRequired")]
    public string PreparationRequired { get; set; } = "";

    [BsonElement("durationTime")]
    public string DurationTime { get; set; } = "";

    [BsonElement("deliveryDeadline")]
    public string DeliveryDeadline { get; set; } = "";

    [BsonElement("description")]
    public string Description { get; set; } = "";

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
