using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class Provincia
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }
} 