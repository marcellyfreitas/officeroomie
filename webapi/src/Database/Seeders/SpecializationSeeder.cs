using WebApi.Models;
using MongoDB.Driver;

namespace WebApi.Database.Seeders;

public class SpecializationSeeder : ISeeder
{
    private readonly IApplicationDbContext _context;

    public SpecializationSeeder(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        var collection = _context.Specializations;

        var exists = await collection.Find(Builders<Specialization>.Filter.Empty).AnyAsync();
        if (exists) return;

        var specializations = new List<Specialization>
        {
            new Specialization { Name = "Clínica Geral", Description = "Atendimento médico geral, prevenção e acompanhamento de saúde." },
            new Specialization { Name = "Pediatria", Description = "Atendimento médico de crianças e adolescentes." },
            new Specialization { Name = "Ginecologia e Obstetrícia", Description = "Saúde da mulher, gravidez e parto." },
            new Specialization { Name = "Cardiologia", Description = "Doenças e prevenção do coração e sistema circulatório." },
            new Specialization { Name = "Ortopedia", Description = "Diagnóstico e tratamento de ossos, músculos e articulações." },
            new Specialization { Name = "Dermatologia", Description = "Cuidados com a pele, cabelo e unhas." },
            new Specialization { Name = "Oftalmologia", Description = "Saúde dos olhos e visão." },
            new Specialization { Name = "Endocrinologia", Description = "Tratamento de distúrbios hormonais e metabólicos." },
            new Specialization { Name = "Neurologia", Description = "Doenças do sistema nervoso e cérebro." },
            new Specialization { Name = "Psiquiatria", Description = "Tratamento de transtornos mentais e comportamentais." }
        };

        await collection.InsertManyAsync(specializations);
    }
}
