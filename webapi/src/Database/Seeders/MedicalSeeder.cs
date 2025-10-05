using WebApi.Helpers;
using WebApi.Models;
using Bogus;
using MongoDB.Driver;
using MongoDB.Bson;

namespace WebApi.Database.Seeders;

public class MedicalSeeder : ISeeder
{
    private readonly IApplicationDbContext _context;
    public MedicalSeeder(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        var collection = _context.Medicals;

        var existingMedical = await collection
            .Find(m => m.Email == "medico@email.com")
            .FirstOrDefaultAsync();

        // busca todas as especializações do banco
        var specializations = await _context.Specializations
            .Find(Builders<Specialization>.Filter.Empty)
            .ToListAsync();

        if (!specializations.Any())
            throw new Exception("Não existem especializações no banco. Rode o SpecializationSeeder antes.");

        var random = new Random();

        if (existingMedical == null)
        {
            var medicalFaker = new Faker<Medical>()
                .RuleFor(m => m.Name, f => "Dr. House")
                .RuleFor(m => m.Email, f => "medico@email.com")
                .RuleFor(m => m.Cpf, f => CpfHelper.Generate())
                .RuleFor(m => m.Crm, f => f.Random.Number(10000, 99999).ToString())
                // escolhe uma especialização aleatória existente
                .RuleFor(m => m.SpecializationId, f => specializations[random.Next(specializations.Count)].Id)
                .RuleFor(m => m.CreatedAt, f => DateTime.UtcNow)
                .RuleFor(m => m.UpdatedAt, f => DateTime.UtcNow);

            var medical = medicalFaker.Generate();
            await collection.InsertOneAsync(medical);

            await SeedFakeMedicals(10, specializations);
        }
    }

    private async Task SeedFakeMedicals(int quantity, List<Specialization> specializations)
    {
        var collection = _context.Medicals;

        var existingEmails = (await collection
            .Find(Builders<Medical>.Filter.Empty)
            .Project(m => m.Email)
            .ToListAsync())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var random = new Random();

        var medicalFaker = new Faker<Medical>("pt_BR")
            .RuleFor(m => m.Name, f => f.Name.FullName())
            .RuleFor(m => m.Email, (f, m) =>
            {
                string email;
                do
                {
                    email = f.Internet.Email(m.Name.ToLower());
                } while (existingEmails.Contains(email));

                existingEmails.Add(email);
                return email;
            })
            .RuleFor(m => m.Cpf, f => CpfHelper.Generate())
            .RuleFor(m => m.Crm, f => f.Random.Number(10000, 99999).ToString())
            .RuleFor(m => m.SpecializationId, f => specializations[random.Next(specializations.Count)].Id)
            .RuleFor(m => m.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(m => m.UpdatedAt, f => DateTime.UtcNow);

        var fakeMedicals = medicalFaker.Generate(quantity);
        await collection.InsertManyAsync(fakeMedicals);
    }
}
