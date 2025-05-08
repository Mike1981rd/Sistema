using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class Vendedor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Display(Name = "Teléfono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Display(Name = "Porcentaje de Comisión")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeComision { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
} 