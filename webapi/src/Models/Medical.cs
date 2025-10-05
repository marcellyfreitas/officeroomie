using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public class Medical
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("name")]
    public string Name { get; set; } = String.Empty;

    [BsonElement("cpf")]
    public string Cpf { get; set; } = String.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = String.Empty;

    [BsonElement("crm")]
    public string Crm { get; set; } = String.Empty;

    [BsonElement("specializationId")]
    public string SpecializationId { get; set; } = String.Empty;

    public Specialization? Specialization { get; set; } = null;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}


