using OfficeRoomie.Models;

namespace OfficeRoomie.Database.Seeders;

public class ClienteSeeder
{
    private readonly AppDbContext _context;

    public ClienteSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Clientes.Any())
        {
            var clientes = new Cliente[]
            {
                new Cliente { nome = "João Silva", email = "joao.silva@email.com", cpf = "123.456.789-01" },
                new Cliente { nome = "Maria Souza", email = "maria.souza@email.com", cpf = "123.456.789-02" },
                new Cliente { nome = "Carlos Oliveira", email = "carlos.oliveira@email.com", cpf = "123.456.789-03" },
                new Cliente { nome = "Ana Lima", email = "ana.lima@email.com", cpf = "123.456.789-04" },
                new Cliente { nome = "Pedro Almeida", email = "pedro.almeida@email.com", cpf = "123.456.789-05" },
                new Cliente { nome = "Lucas Fernandes", email = "lucas.fernandes@email.com", cpf = "123.456.789-06" },
                new Cliente { nome = "Larissa Castro", email = "larissa.castro@email.com", cpf = "123.456.789-07" },
                new Cliente { nome = "Rafael Santos", email = "rafael.santos@email.com", cpf = "123.456.789-08" },
                new Cliente { nome = "Fernanda Costa", email = "fernanda.costa@email.com", cpf = "123.456.789-09" },
                new Cliente { nome = "Bruno Machado", email = "bruno.machado@email.com", cpf = "123.456.789-10" },
                new Cliente { nome = "Carla Nunes", email = "carla.nunes@email.com", cpf = "123.456.789-11" },
                new Cliente { nome = "Felipe Moreira", email = "felipe.moreira@email.com", cpf = "123.456.789-12" },
                new Cliente { nome = "Paula Ribeiro", email = "paula.ribeiro@email.com", cpf = "123.456.789-13" },
                new Cliente { nome = "Gustavo Pires", email = "gustavo.pires@email.com", cpf = "123.456.789-14" },
                new Cliente { nome = "Isabela Duarte", email = "isabela.duarte@email.com", cpf = "123.456.789-15" },
                new Cliente { nome = "Rodrigo Martins", email = "rodrigo.martins@email.com", cpf = "123.456.789-16" },
                new Cliente { nome = "Juliana Araújo", email = "juliana.araujo@email.com", cpf = "123.456.789-17" },
                new Cliente { nome = "Leonardo Barros", email = "leonardo.barros@email.com", cpf = "123.456.789-18" },
                new Cliente { nome = "Amanda Teixeira", email = "amanda.teixeira@email.com", cpf = "123.456.789-19" },
                new Cliente { nome = "Thiago Souza", email = "thiago.souza@email.com", cpf = "123.456.789-20" }
            };

            _context.Clientes.AddRange(clientes);
            _context.SaveChanges();
        }
    }
}
