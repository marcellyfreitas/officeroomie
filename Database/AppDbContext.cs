using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OfficeRoomie.Models;

namespace OfficeRoomie.Database;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration): base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = optionsBuilder.Options.FindExtension<CoreOptionsExtension>()?.ApplicationServiceProvider?.GetService<IConfiguration>();

            if (configuration == null) { 
                throw new Exception("Arquivo de configuração não encontrado.");
            }

            var databaseProvider = configuration.GetValue<string>("DatabaseProvider");

            switch (databaseProvider)
            {
                case "Localdb":
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                    break;
                case "SQLite":
                    optionsBuilder.UseSqlite(_configuration.GetConnectionString("SQLiteConnection"));
                    break;
                case "MySql":
                    optionsBuilder.UseMySQL(configuration.GetConnectionString("MySqlConnection")!);
                    break;
                case "SqlServer":
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")!);
                    break; 
                case "Production":
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("SmarterAspMSSQL")!);
                    break;
                default:
                    throw new Exception($"Provedor de banco de dados não suportado: {databaseProvider}");
            }
        }
    }

    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Administrador> Administradores { get; set; }
    public DbSet<Reserva> Reservas { get; set; }

    public DbSet<Cartao> Cartoes { get; set; }

    public DbSet<Sala> Salas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(e => e.Property(a => a.id).ValueGeneratedOnAdd());
        modelBuilder.Entity<Sala>(e => e.Property(s => s.id).ValueGeneratedOnAdd());
        modelBuilder.Entity<Cliente>(e => e.Property(c => c.id).ValueGeneratedOnAdd());
        modelBuilder.Entity<Reserva>(e => e.Property(r => r.id).ValueGeneratedOnAdd());
        modelBuilder.Entity<Cartao>(e => e.Property(c => c.id).ValueGeneratedOnAdd());
    }
}
