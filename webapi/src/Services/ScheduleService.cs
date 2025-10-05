using MongoDB.Driver;
using WebApi.Database;
using WebApi.Models;

namespace WebApi.Services
{
    public class ScheduleService : IBaseService<Schedule>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<ScheduleService> _logger;

        public ScheduleService(IApplicationDbContext context, ILogger<ScheduleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            try
            {
                var list = await _context.Schedules
                    .Find(_ => true)
                    .SortByDescending(u => u.CreatedAt)
                    .ToListAsync();

                if (list.Count > 0)
                {
                    var relationshipIds = list.Select(d => d.MedicalId).Distinct().ToList();

                    var relationship = await _context.Medicals
                        .Find(s => relationshipIds.Contains(s.Id))
                        .ToListAsync();

                    foreach (var model in list)
                    {
                        model.Medical = relationship.FirstOrDefault(s => s.Id == model.MedicalId);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os models");
                throw;
            }
        }

        public async Task<Schedule?> GetByIdAsync(string id)
        {
            try
            {
                var filter = Builders<Schedule>.Filter.Eq(u => u.Id, id);
                var model = await _context.Schedules.Find(filter).FirstOrDefaultAsync();

                if (model != null && !string.IsNullOrEmpty(model.MedicalId))
                {
                    model.Medical = await _context.Medicals
                        .Find(s => s.Id == model.MedicalId)
                        .FirstOrDefaultAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar model por Id: {id}");
                throw;
            }
        }

        public async Task<Schedule?> AddAsync(Schedule model)
        {
            try
            {
                await _context.Schedules.InsertOneAsync(model);
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir model");
                throw;
            }
        }

        public async Task UpdateAsync(Schedule model)
        {
            try
            {
                var filter = Builders<Schedule>.Filter.Eq(u => u.Id, model.Id);
                await _context.Schedules.ReplaceOneAsync(filter, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar model: {model.Id}");
                throw;
            }
        }

        public async Task DeleteAsync(Schedule model)
        {
            try
            {
                var filter = Builders<Schedule>.Filter.Eq(u => u.Id, model.Id);
                await _context.Schedules.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar model: {model.Id}");
                throw;
            }
        }
    }
}
