using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImobiliariaMathers.Models
{
    public enum TipoUsuario
    {
        USER,
        ADMINISTRATOR
    }

    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("senha")]
        public string Senha { get; set; } = string.Empty;

        [Required]
        [Column("tipo")]
        public TipoUsuario Tipo { get; set; } = TipoUsuario.USER;

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }
    }
}