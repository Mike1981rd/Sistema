using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    /// <summary>
    /// Tabla de relación muchos-a-muchos entre ProductoVentaPrecio e Impuesto
    /// Permite que cada nivel de precio tenga impuestos específicos
    /// Si un precio no tiene impuestos específicos, hereda los del producto
    /// </summary>
    public class ProductoVentaPrecioImpuesto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoVentaPrecioId { get; set; }
        [ForeignKey("ProductoVentaPrecioId")]
        public virtual ProductoVentaPrecio ProductoVentaPrecio { get; set; } = null!;

        [Required]
        public int ImpuestoId { get; set; }
        [ForeignKey("ImpuestoId")]
        public virtual Impuesto Impuesto { get; set; } = null!;

        /// <summary>
        /// Orden de aplicación del impuesto en este nivel de precio
        /// </summary>
        public int Orden { get; set; } = 0;

        /// <summary>
        /// Permite override del porcentaje del impuesto para este precio específico
        /// Si es null, usa el porcentaje del impuesto base
        /// </summary>
        [Column(TypeName = "decimal(5,4)")]
        public decimal? PorcentajeOverride { get; set; }

        /// <summary>
        /// Notas específicas sobre este impuesto en este nivel de precio
        /// </summary>
        [StringLength(255)]
        public string? Notas { get; set; }

        /// <summary>
        /// Estado activo/inactivo
        /// </summary>
        public bool Activo { get; set; } = true;

        // Campos de auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        /// <summary>
        /// Obtiene el porcentaje efectivo del impuesto
        /// (override si existe, sino el porcentaje base del impuesto)
        /// </summary>
        public decimal ObtenerPorcentajeEfectivo()
        {
            return PorcentajeOverride ?? Impuesto?.Porcentaje ?? 0m;
        }
    }
}