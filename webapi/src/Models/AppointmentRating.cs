using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public class AppointmentRating
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("rating")]
    public int Rating { get; set; } = 0;

    [BsonElement("comment")]
    public string Comment { get; set; } = String.Empty;

    [BsonElement("appointmentId")]
    public string AppointmentId { get; set; } = String.Empty;

    [BsonElement("userId")]
    public string UserId { get; set; } = String.Empty;

    public Appointment? Appointment { get; set; } = null;

    public User? User { get; set; } = null;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}