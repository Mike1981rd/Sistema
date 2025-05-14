using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class Compra : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual Cliente Proveedor { get; set; }

        [Display(Name = "Almacén")]
        public int? AlmacenId { get; set; }

        [ForeignKey("AlmacenId")]
        public virtual Almacen Almacen { get; set; }

        [StringLength(50)]
        [Display(Name = "Referencia")]
        public string Referencia { get; set; }

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [Display(Name = "Plazo de Pago")]
        public int? PlazoPagoId { get; set; }

        [ForeignKey("PlazoPagoId")]
        public virtual PlazoPago PlazoPago { get; set; }

        [Display(Name = "Fecha de Vencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Descuento")]
        public decimal Descuento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Impuestos")]
        public decimal Impuestos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Recibido, Anulado

        [Display(Name = "Entrada de Diario")]
        public int? EntradaDiarioId { get; set; }

        [ForeignKey("EntradaDiarioId")]
        public virtual EntradaDiario EntradaDiario { get; set; }

        // Multiempresa
        public int EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual Empresa Empresa { get; set; }

        // Navegación a detalles
        public virtual ICollection<CompraDetalle> Detalles { get; set; } = new List<CompraDetalle>();
    }
} 