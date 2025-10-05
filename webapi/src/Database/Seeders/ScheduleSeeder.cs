using WebApi.Helpers;
using WebApi.Models;
using Bogus;
using MongoDB.Driver;

namespace WebApi.Database.Seeders;

public class ScheduleSeeder : ISeeder
{
    private readonly IApplicationDbContext _context;
    public ScheduleSeeder(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        var schedulesCollection = _context.Schedules;
        var medicalsCollection = _context.Medicals;

        var now = DateTime.UtcNow;
        var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var exists = await schedulesCollection.Find(x =>
            x.InitialHour >= firstDayOfMonth && x.InitialHour <= lastDayOfMonth
        ).AnyAsync();

        if (exists) return;

        var medicals = await medicalsCollection.Find(_ => true).ToListAsync();

        var schedulesToInsert = new List<Schedule>();

        foreach (var medical in medicals)
        {
            for (var date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;

                var hours = new List<(int start, int end)>
                {
                    (9, 10),
                    (11, 12),
                    (14, 15),
                    (16, 17)
                };

                foreach (var (start, end) in hours)
                {
                    schedulesToInsert.Add(new Schedule
                    {
                        InitialHour = new DateTime(date.Year, date.Month, date.Day, start, 0, 0, DateTimeKind.Utc),
                        FinalHour = new DateTime(date.Year, date.Month, date.Day, end, 0, 0, DateTimeKind.Utc),
                        MedicalId = medical.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }
        }

        if (schedulesToInsert.Any())
        {
            await schedulesCollection.InsertManyAsync(schedulesToInsert);
        }
    }
}
