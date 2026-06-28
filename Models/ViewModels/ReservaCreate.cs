namespace OfficeRoomie.Models.ViewModels;

public class ReservaCreate
{
    public Reserva reserva { get; set; }
    public List<Cliente> clientes { get; set; } = new List<Cliente>();
    public List<Sala> salas { get; set; } = new List<Sala>();
    public List<Cartao> cartoes { get; set; } = new List<Cartao>();
}
