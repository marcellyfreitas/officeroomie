using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services;

public interface IBaseService<T>
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(string id);

    Task<T?> AddAsync(T data);

    Task UpdateAsync(T data);

    Task DeleteAsync(T data);
}