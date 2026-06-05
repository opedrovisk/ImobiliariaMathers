using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImobiliariaMathers.Models
{
    [Table("codigos_recuperacao")]
    public class CodigoRecuperacao
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("usuario_id")]
        public long UsuarioId { get; set; }

        [Required]
        [MaxLength(64)]
        [Column("codigo_hash")]
        public string CodigoHash { get; set; } = string.Empty;

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }

        [Column("usado_em")]
        public DateTime? UsadoEm { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}
