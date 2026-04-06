using System.ComponentModel.DataAnnotations;

namespace ImobiliariaViviane.Models
{
    public class CadastroImovelRequest
    {
        [Required]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        public string Negocio { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Bairro { get; set; }

        [Required]
        [MaxLength(100)]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Estado { get; set; } = string.Empty;

        [MaxLength(9)]
        public string? Cep { get; set; }

        [Range(1, 99)]
        public byte Dormitorios { get; set; }

        public bool Garagem { get; set; }

        [Range(typeof(decimal), "1", "99999999")]
        public decimal AreaM2 { get; set; }

        [Range(typeof(decimal), "1", "999999999999")]
        public decimal Preco { get; set; }

        public string? Descricao { get; set; }

        public List<string>? Imagens { get; set; }
    }
}