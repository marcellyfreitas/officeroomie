using OfficeRoomie.Helpers;
using OfficeRoomie.Models;

namespace OfficeRoomie.Database.Seeders;

public class AdministradorSeeder
{
    private readonly AppDbContext _context;

    public AdministradorSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Administradores.Any(a => a.email == "email@email.com"))
        {
            var senha = PasswordHelper.HashPassword("123");
            var administrador = new Administrador { nome = "Admin", email = "email@email.com", senha = "" };
            administrador.senha = senha;

            _context.Administradores.Add(administrador);
            _context.SaveChanges();
        }
    }
}
