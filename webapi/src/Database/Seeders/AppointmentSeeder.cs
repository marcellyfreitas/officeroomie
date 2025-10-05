using WebApi.Models;
using Bogus;
using MongoDB.Driver;

namespace WebApi.Database.Seeders;

public class AppointmentSeeder : ISeeder
{
    private readonly IApplicationDbContext _context;
    public AppointmentSeeder(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        var appointmentsCollection = _context.Appointments;
        var usersCollection = _context.Users;
        var schedulesCollection = _context.Schedules;

        var now = DateTime.UtcNow;
        var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var existingAppointments = await appointmentsCollection.Find(a =>
            a.CreatedAt >= firstDayOfMonth && a.CreatedAt <= lastDayOfMonth
        ).AnyAsync();

        if (existingAppointments) return;

        var users = await usersCollection.Find(Builders<User>.Filter.Empty).ToListAsync();
        var schedules = await schedulesCollection.Find(Builders<Schedule>.Filter.Empty).ToListAsync();

        if (!users.Any() || !schedules.Any())
            throw new Exception("Não existem usuários ou schedules para criar appointments.");

        var halfSchedulesCount = schedules.Count / 2;
        var random = new Random();

        var selectedSchedules = schedules.OrderBy(_ => random.Next()).Take(halfSchedulesCount).ToList();

        var statusOptions = new[] { "Agendado", "Cancelado", "Concluído" };
        var appointmentsToInsert = new List<Appointment>();

        foreach (var schedule in selectedSchedules)
        {
            var user = users[random.Next(users.Count)];

            var appointment = new Appointment
            {
                UserId = user.Id,
                ScheduleId = schedule.Id,
                Status = statusOptions[random.Next(statusOptions.Length)],
                Description = $"Consulta de {user.Name} com {schedule.Medical?.Name ?? "médico"}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            appointmentsToInsert.Add(appointment);
        }

        if (appointmentsToInsert.Any())
        {
            await appointmentsCollection.InsertManyAsync(appointmentsToInsert);
        }
    }
}
