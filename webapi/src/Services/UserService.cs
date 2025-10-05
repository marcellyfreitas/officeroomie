using MongoDB.Driver;
using WebApi.Database;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Services
{
    public class UserService : IUserService<User>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(IApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.Users.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os usuários");
                throw;
            }
        }

        public async Task<(IEnumerable<User> Users, long TotalCount)> GetFilteredAsync(string? name = null, string? email = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var filterBuilder = Builders<User>.Filter;
                var filters = new List<FilterDefinition<User>>();

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

                var collection = _context.Users;

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
                _logger.LogError(ex, "Erro ao buscar usuários filtrados");
                throw;
            }
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, id);
                return await _context.Users.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário por Id: {id}");
                throw;
            }
        }

        public async Task<User?> AddAsync(User user)
        {
            try
            {
                user.Password = PasswordHelper.HashPassword(user.Password);
                await _context.Users.InsertOneAsync(user);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir usuário");
                throw;
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
                await _context.Users.ReplaceOneAsync(filter, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar usuário: {user.Id}");
                throw;
            }
        }

        public async Task DeleteAsync(User user)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
                await _context.Users.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar usuário: {user.Id}");
                throw;
            }
        }

        public async Task<User?> GetByEmailAsync(string email, string? id = null)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(u => u.Email, email);

            if (!string.IsNullOrEmpty(id))
            {
                var excludeFilter = builder.Ne(u => u.Id, id);
                filter = builder.And(filter, excludeFilter);
            }

            return await _context.Users.Find(filter).FirstOrDefaultAsync();
        }

    }
}
