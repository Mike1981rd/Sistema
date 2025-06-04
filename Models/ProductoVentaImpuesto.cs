using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    /// <summary>
    /// Tabla de relación muchos-a-muchos entre ProductoVenta e Impuesto
    /// Permite que un producto tenga múltiples impuestos
    /// </summary>
    public class ProductoVentaImpuesto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoVentaId { get; set; }
        [ForeignKey("ProductoVentaId")]
        public virtual ProductoVenta ProductoVenta { get; set; }

        [Required]
        public int ImpuestoId { get; set; }
        [ForeignKey("ImpuestoId")]
        public virtual Impuesto Impuesto { get; set; }

        /// <summary>
        /// Orden de aplicación del impuesto
        /// </summary>
        public int Orden { get; set; } = 0;

        // Campos de auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public int? UsuarioCreacionId { get; set; }

        [Required]
        public int EmpresaId { get; set; }
    }
}