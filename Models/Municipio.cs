using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class Municipio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Provincia")]
        public int ProvinciaId { get; set; }

        [ForeignKey("ProvinciaId")]
        public Provincia? Provincia { get; set; }
    }
} 