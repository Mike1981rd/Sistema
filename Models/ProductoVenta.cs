using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class ProductoVenta : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        [Required]
        public int ItemContenedorId { get; set; }
        [ForeignKey("ItemContenedorId")]
        public virtual ItemContenedor? ItemContenedor { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Cantidad { get; set; } = 1;

        [Column(TypeName = "decimal(18,4)")]
        public decimal PrecioVenta { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Costo { get; set; }

        // Calculado: Costo * Cantidad
        [Column(TypeName = "decimal(18,4)")]
        public decimal CostoTotal { get; set; }

        public int? ImpuestoId { get; set; }
        [ForeignKey("ImpuestoId")]
        public virtual Impuesto? Impuesto { get; set; }

        // Indica si este producto está disponible para venta
        public bool DisponibleParaVenta { get; set; } = true;

        // Indica si requiere preparación (útil para restaurantes)
        public bool RequierePreparacion { get; set; }

        // Tiempo estimado de preparación (en minutos)
        public int? TiempoPreparacion { get; set; }

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}