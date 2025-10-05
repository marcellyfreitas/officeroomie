using WebApi.Models;

namespace WebApi.Services;

public interface IMedicalService : IBaseService<Medical>
{
    Task<Medical?> GetByEmailAsync(string email, string? id = null);
}