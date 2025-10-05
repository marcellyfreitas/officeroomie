using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services;

public interface IUserService<T> : IBaseService<T>
{
    Task<T?> GetByEmailAsync(string email, string? id = null);

    Task<(IEnumerable<T> Users, long TotalCount)> GetFilteredAsync(string? name = null, string? email = null, int page = 1, int pageSize = 10);
}