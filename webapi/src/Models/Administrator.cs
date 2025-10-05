using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public class Administrator
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("name")]
    public string Name { get; set; } = "";

    [BsonElement("email")]
    public string Email { get; set; } = "";

    [BsonElement("password")]
    public string Password { get; set; } = "";

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

