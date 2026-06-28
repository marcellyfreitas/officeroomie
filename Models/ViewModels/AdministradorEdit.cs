using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeRoomie.Models.ViewModels
{
    [Table("administradores")]
    public class AdministradorEdit
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Preenchimento do Campo 'nome' Obrigatório!")]
        [Display(Name="Nome")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Preenchimento do Campo 'email' Obrigatório!")]
        [Display(Name="E-mail")]
        public string email { get; set; }

        [Display(Name = "Senha")]
        public string? senha { get; set; } = "";

        [Display(Name = "CPF")]
        public string? cpf { get; set; } = "";

        [Display(Name = "Permissões")]
        public string? permissoes { get; set; } = "";

        public string? created_at { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

        public string? updated_at { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
    }
}
