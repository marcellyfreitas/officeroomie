using MongoDB.Driver;
using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Database;

public class ApplicationDbContext : IApplicationDbContext
{
    private readonly IMongoDatabase _database;

    public ApplicationDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDbClusterConnection");
        var mongoUrl = new MongoUrl(connectionString);
        var client = new MongoClient(mongoUrl);

        _database = client.GetDatabase(mongoUrl.DatabaseName ?? "WebApiDb");
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    public IMongoCollection<Administrator> Administrators => _database.GetCollection<Administrator>("administrators");
    public IMongoCollection<Specialization> Specializations => _database.GetCollection<Specialization>("specializations");
    public IMongoCollection<Schedule> Schedules => _database.GetCollection<Schedule>("schedules");
    public IMongoCollection<Appointment> Appointments => _database.GetCollection<Appointment>("appointments");
    public IMongoCollection<Medical> Medicals => _database.GetCollection<Medical>("medicals");
    public IMongoCollection<AppointmentRating> AppointmentRatings => _database.GetCollection<AppointmentRating>("appointmentRatings");
    public IMongoCollection<MedicalExam> MedicalExams => _database.GetCollection<MedicalExam>("medicalExams");
    public IMongoCollection<Convenio> Convenios => _database.GetCollection<Convenio>("convenios");
}