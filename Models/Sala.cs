using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeRoomie.Models
{
    [Table("salas")]
    public class Sala
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Preenchimento do Campo 'nome' Obrigatório!")]
        [Display(Name = "Nome")]
        public string nome { get; set; } = "";

        [Required(ErrorMessage = "Preenchimento do Campo 'descricao' Obrigatório!")]
        [Display(Name = "Descrição")]
        public string descricao { get; set; } = "";

        [Required(ErrorMessage = "Preenchimento do Campo 'capacidade' Obrigatório!")]
        [Display(Name = "Capacidade")]
        public string capacidade { get; set; } = "";

        [Required(ErrorMessage = "Preenchimento do Campo 'categoria' Obrigatório!")]
        [Display(Name = "Categoria")]
        public string categoria { get; set; } = "";

        public string? created_at { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

        public string? updated_at { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
    }
}
