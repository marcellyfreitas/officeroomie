using MongoDB.Driver;
using WebApi.Database;
using WebApi.Models;

namespace WebApi.Services
{
    public class AdministratorService : IUserService<Administrator>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<AdministratorService> _logger;

        public AdministratorService(IApplicationDbContext context, ILogger<AdministratorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Administrator>> GetAllAsync()
        {
            try
            {
                return await _context.Administrators.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os administradores");
                throw;
            }
        }

        public async Task<(IEnumerable<Administrator> Users, long TotalCount)> GetFilteredAsync(string? name = null, string? email = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var filterBuilder = Builders<Administrator>.Filter;
                var filters = new List<FilterDefinition<Administrator>>();

                if (!string.IsNullOrEmpty(name))
                {
                    filters.Add(filterBuilder.Regex(u => u.Name, new MongoDB.Bson.BsonRegularExpression(name, "i")));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    filters.Add(filterBuilder.Regex(u => u.Email, new MongoDB.Bson.BsonRegularExpression(email, "i")));
                }

                var combinedFilter = filters.Count > 0
                    ? filterBuilder.And(filters)
                    : filterBuilder.Empty;

                var collection = _context.Administrators;

                var totalCount = await collection.CountDocumentsAsync(combinedFilter);

                var users = await collection
                    .Find(combinedFilter)
                    .SortByDescending(u => u.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Limit(pageSize)
                    .ToListAsync();

                return (users, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados filtrados");
                throw;
            }
        }

        public async Task<Administrator?> GetByIdAsync(string id)
        {
            try
            {
                var filter = Builders<Administrator>.Filter.Eq(a => a.Id, id);
                return await _context.Administrators.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar administrador por Id: {id}");
                throw;
            }
        }

        public async Task<Administrator?> AddAsync(Administrator admin)
        {
            try
            {
                await _context.Administrators.InsertOneAsync(admin);
                return admin;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir administrador");
                throw;
            }
        }

        public async Task UpdateAsync(Administrator admin)
        {
            try
            {
                var filter = Builders<Administrator>.Filter.Eq(a => a.Id, admin.Id);
                admin.UpdatedAt = DateTime.UtcNow;
                await _context.Administrators.ReplaceOneAsync(filter, admin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar administrador: {admin.Id}");
                throw;
            }
        }

        public async Task DeleteAsync(Administrator admin)
        {
            try
            {
                var filter = Builders<Administrator>.Filter.Eq(a => a.Id, admin.Id);
                await _context.Administrators.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar administrador: {admin.Id}");
                throw;
            }
        }

        public async Task<Administrator?> GetByEmailAsync(string email, string? id = null)
        {
            var builder = Builders<Administrator>.Filter;
            var filter = builder.Eq(u => u.Email, email);

            if (!string.IsNullOrEmpty(id))
            {
                var excludeFilter = builder.Ne(u => u.Id, id);
                filter = builder.And(filter, excludeFilter);
            }

            return await _context.Administrators.Find(filter).FirstOrDefaultAsync();
        }
    }
}
