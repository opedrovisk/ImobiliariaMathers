using System.ComponentModel.DataAnnotations;

namespace ImobiliariaViviane.Models
{
    public class LoginUsuarioRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Senha { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
