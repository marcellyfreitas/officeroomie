using MongoDB.Driver;
using WebApi.Database;
using WebApi.Models;

namespace WebApi.Services
{
    public class SpecializationService : IBaseService<Specialization>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<SpecializationService> _logger;

        public SpecializationService(IApplicationDbContext context, ILogger<SpecializationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Specialization>> GetAllAsync()
        {
            try
            {
                return await _context.Specializations.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar lista");
                throw;
            }
        }

        public async Task<Specialization?> GetByIdAsync(string id)
        {
            try
            {
                var filter = Builders<Specialization>.Filter.Eq(u => u.Id, id);
                return await _context.Specializations.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar por Id: {id}");
                throw;
            }
        }

        public async Task<Specialization?> AddAsync(Specialization Specialization)
        {
            try
            {
                await _context.Specializations.InsertOneAsync(Specialization);
                return Specialization;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir");
                throw;
            }
        }

        public async Task UpdateAsync(Specialization Specialization)
        {
            try
            {
                var filter = Builders<Specialization>.Filter.Eq(u => u.Id, Specialization.Id);
                await _context.Specializations.ReplaceOneAsync(filter, Specialization);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar: {Specialization.Id}");
                throw;
            }
        }

        public async Task DeleteAsync(Specialization Specialization)
        {
            try
            {
                var filter = Builders<Specialization>.Filter.Eq(u => u.Id, Specialization.Id);
                await _context.Specializations.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar: {Specialization.Id}");
                throw;
            }
        }
    }
}
