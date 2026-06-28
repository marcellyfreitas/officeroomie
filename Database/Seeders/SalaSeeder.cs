using OfficeRoomie.Models;

namespace OfficeRoomie.Database.Seeders;

public class SalaSeeder
{
    private readonly AppDbContext _context;

    public SalaSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Salas.Any())
        {
            var salas = new Sala[]
            {
                new Sala { nome = "Sala de Reunião 1", descricao = "Sala equipada com projetor e 10 cadeiras", capacidade = "10", categoria = "Reunião" },
                new Sala { nome = "Sala de Reunião 2", descricao = "Sala de reuniões com TV e 8 cadeiras", capacidade = "8", categoria = "Reunião" },
                new Sala { nome = "Auditório 1", descricao = "Auditório com capacidade para 50 pessoas", capacidade = "50", categoria = "Auditório" },
                new Sala { nome = "Auditório 2", descricao = "Auditório com 100 lugares e sistema de som", capacidade = "100", categoria = "Auditório" },
                new Sala { nome = "Sala de Treinamento 1", descricao = "Sala para workshops com 20 cadeiras", capacidade = "20", categoria = "Treinamento" },
                new Sala { nome = "Sala de Treinamento 2", descricao = "Sala com 25 cadeiras e quadros brancos", capacidade = "25", categoria = "Treinamento" },
                new Sala { nome = "Sala de Conferência 1", descricao = "Sala de videoconferência para 12 pessoas", capacidade = "12", categoria = "Conferência" },
                new Sala { nome = "Sala de Conferência 2", descricao = "Sala para videoconferência com 15 cadeiras", capacidade = "15", categoria = "Conferência" },
                new Sala { nome = "Sala de Aulas 1", descricao = "Sala de aulas com 30 cadeiras e projetor", capacidade = "30", categoria = "Aulas" },
                new Sala { nome = "Sala de Aulas 2", descricao = "Sala para aulas e seminários com 35 lugares", capacidade = "35", categoria = "Aulas" },
                new Sala { nome = "Sala de Workshops", descricao = "Sala para workshops com 20 cadeiras", capacidade = "20", categoria = "Workshop" },
                new Sala { nome = "Sala de Conferências Executiva", descricao = "Sala executiva para videoconferências com 10 cadeiras", capacidade = "10", categoria = "Executiva" },
                new Sala { nome = "Sala Pequena de Reunião", descricao = "Sala para pequenas reuniões, 4 cadeiras", capacidade = "4", categoria = "Reunião" },
                new Sala { nome = "Sala Grande de Reunião", descricao = "Sala de reuniões para grandes equipes com 20 lugares", capacidade = "20", categoria = "Reunião" },
                new Sala { nome = "Sala de Treinamento VIP", descricao = "Sala premium para treinamentos, 15 cadeiras", capacidade = "15", categoria = "Treinamento" },
                new Sala { nome = "Espaço Colaborativo 1", descricao = "Espaço aberto para colaboração, até 10 pessoas", capacidade = "10", categoria = "Colaborativo" },
                new Sala { nome = "Espaço Colaborativo 2", descricao = "Área colaborativa com 12 lugares", capacidade = "12", categoria = "Colaborativo" },
                new Sala { nome = "Sala de Conferências Internacional", descricao = "Sala de conferências para até 20 pessoas", capacidade = "20", categoria = "Conferência" },
                new Sala { nome = "Sala de Reunião VIP", descricao = "Sala de reuniões VIP com 8 cadeiras", capacidade = "8", categoria = "Executiva" },
                new Sala { nome = "Auditório VIP", descricao = "Auditório premium com 60 lugares", capacidade = "60", categoria = "Auditório" }
            };

            _context.Salas.AddRange(salas);
            _context.SaveChanges();
        }
    }
}
