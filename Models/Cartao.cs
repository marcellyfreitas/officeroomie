using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeRoomie.Models
{
    [Table("cartoes")]
    public class Cartao
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Preenchimento do Campo 'Número do Cartão' Obrigatório!")]
        [Display(Name = "Número do Cartão")]
        public string numeroDoCartao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Preenchimento do Campo 'Nome do Titular' Obrigatório!")]
        [Display(Name = "Nome do Titular")]
        public string nomeDoTitular { get; set; } = string.Empty;

        [Required(ErrorMessage = "Preenchimento do Campo 'Validade' Obrigatório!")]
        [Display(Name = "Validade")]
        public DateTime validade { get; set; }

        [Required(ErrorMessage = "Preenchimento do Campo 'CVV' Obrigatório!")]
        [Display(Name = "CVV")]
        public int cvv { get; set; }

        [Required(ErrorMessage = "Preenchimento do Campo [cliente] Obrigatório!")]
        [Display(Name = "Cliente")]
        [ForeignKey("cliente")]
        public int cliente_id { get; set; }

        public Cliente? cliente { get; set; }

        public string? created_at { get; set; } = $"{DateTime.Now:yyyy-MM-dd}";

        public string? updated_at { get; set; } = $"{DateTime.Now:yyyy-MM-dd}";
    }
}
