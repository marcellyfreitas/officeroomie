using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public class Appointment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("userId")]
    public string UserId { get; set; } = "";

    [BsonElement("appointmentScheduleId")]
    public string AppointmentScheduleId { get; set; } = "";

    [BsonElement("status")]
    public string Status { get; set; } = "";

    [BsonElement("description")]
    public string Description { get; set; } = "";

    public User? User { get; set; } = null;

    public AppointmentSchedule? Schedule { get; set; } = null;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
