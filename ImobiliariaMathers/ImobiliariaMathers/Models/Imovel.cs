using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImobiliariaMathers.Models
{
    public enum TipoImovel
    {
        CASA,
        APARTAMENTO
    }

    public enum TipoNegocio
    {
        COMPRA,
        ALUGUEL
    }

    [Table("imoveis")]
    public class Imovel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Required]
        [Column("tipo")]
        public TipoImovel Tipo { get; set; }

        [Required]
        [Column("negocio")]
        public TipoNegocio Negocio { get; set; }

        [MaxLength(100)]
        [Column("bairro")]
        public string? Bairro { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("cidade")]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [MaxLength(2)]
        [Column("estado", TypeName = "char(2)")]
        public string Estado { get; set; } = string.Empty;

        [MaxLength(9)]
        [Column("cep")]
        public string? Cep { get; set; }

        [Required]
        [Column("preco", TypeName = "decimal(12,2)")]
        public decimal Preco { get; set; }

        [Required]
        [Column("garagem")]
        public bool Garagem { get; set; }

        [Required]
        [Column("dormitorios")]
        public byte Dormitorios { get; set; }

        [Required]
        [Column("area_m2", TypeName = "decimal(8,2)")]
        public decimal AreaM2 { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; }

        [Column("atualizado_em")]
        public DateTime? AtualizadoEm { get; set; }

        public ICollection<Imagem> Imagens { get; set; } = new List<Imagem>();
    }
}