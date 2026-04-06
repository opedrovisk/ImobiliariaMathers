using System.ComponentModel.DataAnnotations;

namespace ImobiliariaViviane.Models
{
    public class RedefinirSenhaComCodigoRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(32)]
        public string CodigoRecuperacao { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [MaxLength(255)]
        public string NovaSenha { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [MaxLength(255)]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
