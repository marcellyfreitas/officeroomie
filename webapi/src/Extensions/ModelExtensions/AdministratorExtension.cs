using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Extensions.ModelExtensions;

public static class AdministratorExtension
{
    public static AdministratorViewModel ToViewModel(this Administrator user)
    {
        return new AdministratorViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };
    }
}