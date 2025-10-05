using MongoDB.Driver;
using WebApi.Database;
using WebApi.Models;

namespace WebApi.Services
{
    public class MedicalExamService : IBaseService<MedicalExam>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<MedicalExamService> _logger;

        public MedicalExamService(IApplicationDbContext context, ILogger<MedicalExamService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<MedicalExam>> GetAllAsync()
        {
            try
            {
                return await _context.MedicalExams.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar lista");
                throw;
            }
        }

        public async Task<MedicalExam?> GetByIdAsync(string id)
        {
            try
            {
                var filter = Builders<MedicalExam>.Filter.Eq(u => u.Id, id);
                return await _context.MedicalExams.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar por Id: {id}");
                throw;
            }
        }

        public async Task<MedicalExam?> AddAsync(MedicalExam MedicalExam)
        {
            try
            {
                await _context.MedicalExams.InsertOneAsync(MedicalExam);
                return MedicalExam;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir");
                throw;
            }
        }

        public async Task UpdateAsync(MedicalExam MedicalExam)
        {
            try
            {
                var filter = Builders<MedicalExam>.Filter.Eq(u => u.Id, MedicalExam.Id);
                await _context.MedicalExams.ReplaceOneAsync(filter, MedicalExam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar: {MedicalExam.Id}");
                throw;
            }
        }

        public async Task DeleteAsync(MedicalExam MedicalExam)
        {
            try
            {
                var filter = Builders<MedicalExam>.Filter.Eq(u => u.Id, MedicalExam.Id);
                await _context.MedicalExams.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar: {MedicalExam.Id}");
                throw;
            }
        }
    }
}
