using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImobiliariaViviane.Models
{
    [Table("imagens")]
    public class Imagem
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("imovel_id")]
        public long ImovelId { get; set; }

        [Required]
        [Column("url", TypeName = "longtext")]
        public string Url { get; set; } = string.Empty;

        [Column("ordem")]
        public byte Ordem { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }

        [ForeignKey(nameof(ImovelId))]
        public Imovel? Imovel { get; set; }
    }
}