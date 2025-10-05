using MongoDB.Driver;
using WebApi.Database;
using WebApi.Models;

namespace WebApi.Services;

public class ConvenioService : IBaseService<Convenio>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ConvenioService> _logger;

    public ConvenioService(IApplicationDbContext context, ILogger<ConvenioService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Convenio>> GetAllAsync()
    {
        try
        {
            return await _context.Convenios.Find(_ => true).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar lista");
            throw;
        }
    }

    public async Task<Convenio?> GetByIdAsync(string id)
    {
        try
        {
            return await _context.Convenios.Find(c => c.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao buscar model por Id: {id}");
            throw;
        }

    }

    public async Task<Convenio?> AddAsync(Convenio convenio)
    {
        try
        {
            await _context.Convenios.InsertOneAsync(convenio);
            return convenio;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao inserir model");
            throw;
        }
    }

    public async Task UpdateAsync(Convenio convenio)
    {
        try
        {
            var filter = Builders<Convenio>.Filter.Eq(u => u.Id, convenio.Id);
            await _context.Convenios.ReplaceOneAsync(filter, convenio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar model");
            throw;
        }

    }

    public async Task DeleteAsync(Convenio convenio)
    {
        try
        {
            var filter = Builders<Convenio>.Filter.Eq(u => u.Id, convenio.Id);
            await _context.Convenios.DeleteOneAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar model");
            throw;
        }
    }
}