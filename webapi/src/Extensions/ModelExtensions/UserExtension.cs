using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class UserExtension
{
    public static UserViewModel ToViewModel(this User user)
    {
        return new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Cpf = user.Cpf,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };
    }
}