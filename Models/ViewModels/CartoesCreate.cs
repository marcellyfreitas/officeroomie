namespace OfficeRoomie.Models.ViewModels;

public class CartoesCreate
{
    public Cartao cartao { get; set; }
    public List<Cliente> clientes { get; set; } = new List<Cliente>();
}
