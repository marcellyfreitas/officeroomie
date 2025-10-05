using MongoDB.Driver;
using WebApi.Models;

public interface IApplicationDbContext
{
    IMongoCollection<User> Users { get; }
    IMongoCollection<Administrator> Administrators { get; }
    IMongoCollection<Specialization> Specializations { get; }
    IMongoCollection<Schedule> Schedules { get; }
    IMongoCollection<Appointment> Appointments { get; }
    IMongoCollection<Medical> Medicals { get; }
    IMongoCollection<AppointmentRating> AppointmentRatings { get; }
    IMongoCollection<MedicalExam> MedicalExams { get; }
    IMongoCollection<Convenio> Convenios { get; }
}