using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class Pais
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del país es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(2)]
        [Display(Name = "Código ISO")]
        public string? Codigo { get; set; }

        [StringLength(200)]
        [Display(Name = "URL de la Bandera")]
        public string? Bandera { get; set; }
    }
} 