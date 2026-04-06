using System.ComponentModel.DataAnnotations;

namespace ImobiliariaViviane.Models
{
    public class CadastroUsuarioRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [MaxLength(255)]
        public string Senha { get; set; } = string.Empty;
    }
}
