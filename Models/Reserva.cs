using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeRoomie.Models;

[Table("reservas")]
public class Reserva

{
    [Key]
    public int id { get; set; }

    [Required(ErrorMessage = "Preenchimento do Campo [hora inicial] Obrigatório!")]
    [Display(Name="Hora Início")]
    public string hora_inicio { get; set; } = "";
    
    [Required(ErrorMessage = "Preenchimento do Campo [hora final] Obrigatório!")]
    [Display(Name="Hora Fim")]
    public string hora_fim { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo [data da reserva] Obrigatório!")]
    [Display(Name="Data da Reserva")]
    public string data_reserva { get; set; } = "";

    [Display(Name="Protocolo")]
    public string protocolo { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo [status] Obrigatório!")]
    [Display(Name="Status")]
    public string status { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo [sala] Obrigatório!")]
    [Display(Name = "Sala")]
    [ForeignKey("sala")]
    public int sala_id { get; set; }

    [Required(ErrorMessage = "Preenchimento do Campo [cliente] Obrigatório!")]
    [Display(Name = "Cliente")]
    [ForeignKey("cliente")]
    public int cliente_id { get; set; }

    [Display(Name = "Cartao")]
    [ForeignKey("cartao")]
    public int? cartao_id { get; set; }

    public Cliente? cliente { get; set; }
    public Sala? sala { get; set; }
    public Cartao? cartao { get; set; }


    public string? created_at { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
    public string? updated_at { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

}
