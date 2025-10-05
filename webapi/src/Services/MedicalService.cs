using MongoDB.Driver;
using WebApi.Database;
using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Services;

public class MedicalService : IMedicalService
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<MedicalService> _logger;

    public MedicalService(IApplicationDbContext context, ILogger<MedicalService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Medical>> GetAllAsync()
    {
        try
        {
            var list = await _context.Medicals
                    .Find(_ => true)
                    .SortByDescending(u => u.CreatedAt)
                    .ToListAsync();

            if (list.Count > 0)
            {
                var relationshipIds = list.Select(d => d.SpecializationId).Distinct().ToList();

                var relationships = await _context.Specializations
                    .Find(s => relationshipIds.Contains(s.Id))
                    .ToListAsync();

                foreach (var doctor in list)
                {
                    doctor.Specialization = relationships.FirstOrDefault(s => s.Id == doctor.SpecializationId);
                }
            }

            return list;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erro ao buscar todos os usuários");
            throw;
        }
    }

    public async Task<Medical?> GetByIdAsync(string id)
    {
        try
        {
            var filter = Builders<Medical>.Filter.Eq(u => u.Id, id);
            var model = await _context.Medicals.Find(filter).FirstOrDefaultAsync();

            if (model != null && !string.IsNullOrEmpty(model.SpecializationId))
            {
                model.Specialization = await _context.Specializations
                    .Find(s => s.Id == model.SpecializationId)
                    .FirstOrDefaultAsync();
            }

            return model;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error ao buscar um medico");
            throw;
        }
    }

    public async Task<Medical?> AddAsync(Medical data)
    {
        try
        {
            await _context.Medicals.InsertOneAsync(data);
            return data;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erro ao adicionar um medico");
            throw;
        }
    }

    public async Task UpdateAsync(Medical data)
    {
        try
        {
            var filter = Builders<Medical>.Filter.Eq(_ => _.Id, data.Id);
            await _context.Medicals.ReplaceOneAsync(filter, data);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erro ao atualizar um medico");
            throw;
        }
    }

    public async Task DeleteAsync(Medical data)
    {
        try
        {
            var filter = Builders<Medical>.Filter.Eq(_ => _.Id, data.Id);
            await _context.Medicals.DeleteOneAsync(filter);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Erro ao deletar medico: {data.Id}");
            throw;
        }
    }

    public async Task<Medical?> GetByEmailAsync(string email, string? id = null)
    {
        var builder = Builders<Medical>.Filter;
        var filter = builder.Eq(u => u.Email, email);

        if (!string.IsNullOrEmpty(id))
        {
            var excludeFilter = builder.Ne(u => u.Id, id);
            filter = builder.And(filter, excludeFilter);
        }

        return await _context.Medicals.Find(filter).FirstOrDefaultAsync();
    }
}