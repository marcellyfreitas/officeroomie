using WebApi.Helpers;
using WebApi.Models;
using Bogus;
using MongoDB.Driver;

namespace WebApi.Database.Seeders;

public class AdministratorSeeder : ISeeder
{
    private readonly IApplicationDbContext _context;
    public AdministratorSeeder(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        var collection = _context.Administrators;

        var existingAdmin = await collection
            .Find(a => a.Email == "admin@email.com")
            .FirstOrDefaultAsync();

        if (existingAdmin == null)
        {
            var adminFaker = new Faker<Administrator>()
                .RuleFor(a => a.Name, f => "Admin Principal")
                .RuleFor(a => a.Email, f => "admin@email.com")
                .RuleFor(a => a.Password, f => PasswordHelper.HashPassword("senha"))
                .RuleFor(a => a.CreatedAt, f => DateTime.UtcNow)
                .RuleFor(a => a.UpdatedAt, f => DateTime.UtcNow);

            var admin = adminFaker.Generate();
            await collection.InsertOneAsync(admin);

            await SeedFakeAdmins(5);
        }
    }

    private async Task SeedFakeAdmins(int quantity)
    {
        var collection = _context.Administrators;

        var existingEmails = (await collection
            .Find(Builders<Administrator>.Filter.Empty)
            .Project(a => a.Email)
            .ToListAsync())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var adminFaker = new Faker<Administrator>("pt_BR")
            .RuleFor(a => a.Name, f => f.Name.FullName())
            .RuleFor(a => a.Email, (f, a) =>
            {
                string email;
                do
                {
                    email = f.Internet.Email(a.Name.ToLower());
                } while (existingEmails.Contains(email));

                existingEmails.Add(email);
                return email;
            })
            .RuleFor(a => a.Password, f => PasswordHelper.HashPassword("senha"))
            .RuleFor(a => a.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(a => a.UpdatedAt, f => DateTime.UtcNow);

        var fakeAdmins = adminFaker.Generate(quantity);
        await collection.InsertManyAsync(fakeAdmins);
    }
}
