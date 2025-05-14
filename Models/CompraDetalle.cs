using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class CompraDetalle : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompraId { get; set; }

        [ForeignKey("CompraId")]
        public virtual Compra Compra { get; set; }

        [Required]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        [StringLength(150)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Porcentaje de Descuento")]
        public decimal PorcentajeDescuento { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Monto de Descuento")]
        public decimal MontoDescuento { get; set; }

        public int? ImpuestoId { get; set; }

        [ForeignKey("ImpuestoId")]
        public virtual Impuesto Impuesto { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Monto de Impuesto")]
        public decimal MontoImpuesto { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Unidad de Medida")]
        public int UnidadMedidaId { get; set; }

        [ForeignKey("UnidadMedidaId")]
        public virtual UnidadMedida UnidadMedida { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        [Display(Name = "Factor de Conversión")]
        public decimal FactorConversion { get; set; } = 1;

        // Multiempresa
        public int EmpresaId { get; set; }
    }
} 