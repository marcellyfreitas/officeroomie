using WebApi.Helpers;
using WebApi.Models;
using Bogus;
using MongoDB.Driver;

namespace WebApi.Database.Seeders;

public class UserSeeder : ISeeder
{
    private readonly IApplicationDbContext _context;
    public UserSeeder(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        var collection = _context.Users;

        var existingUser = await collection
            .Find(u => u.Email == "user@email.com")
            .FirstOrDefaultAsync();

        if (existingUser == null)
        {
            var userFaker = new Faker<User>()
                .RuleFor(a => a.Name, f => "Jon Snow")
                .RuleFor(a => a.Email, f => "user@email.com")
                .RuleFor(a => a.Cpf, f => CpfHelper.Generate())
                .RuleFor(a => a.Password, f => PasswordHelper.HashPassword("senha"))
                .RuleFor(a => a.CreatedAt, f => DateTime.UtcNow)
                .RuleFor(a => a.UpdatedAt, f => DateTime.UtcNow);

            var user = userFaker.Generate();
            await collection.InsertOneAsync(user);

            await SeedFakeUsers(10);
        }
    }

    private async Task SeedFakeUsers(int quantity)
    {
        var collection = _context.Users;

        var existingEmails = (await collection
             .Find(Builders<User>.Filter.Empty)
             .Project(u => u.Email)
             .ToListAsync())
             .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var userFaker = new Faker<User>("pt_BR")
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, (f, u) =>
            {
                string email;
                do
                {
                    email = f.Internet.Email(u.Name.ToLower());
                } while (existingEmails.Contains(email));

                existingEmails.Add(email);
                return email;
            })
            .RuleFor(u => u.Password, f => PasswordHelper.HashPassword("senha"))
            .RuleFor(u => u.Cpf, f => CpfHelper.Generate())
            .RuleFor(u => u.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(u => u.UpdatedAt, f => DateTime.UtcNow);

        var fakeUsers = userFaker.Generate(quantity);
        await collection.InsertManyAsync(fakeUsers);
    }
}
