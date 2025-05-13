using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class ItemProveedor : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        [Required]
        public int ProveedorId { get; set; }
        [ForeignKey("ProveedorId")]
        // Cambiado de Proveedor a Cliente para unificar el modelo
        public virtual Cliente? Proveedor { get; set; }

        [StringLength(100)]
        public string? NombreCompra { get; set; }

        [StringLength(50)]
        public string? CodigoProveedor { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal PrecioCompra { get; set; }

        public int UnidadMedidaCompraId { get; set; }
        [ForeignKey("UnidadMedidaCompraId")]
        public virtual UnidadMedida? UnidadMedidaCompra { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal FactorConversion { get; set; } = 1;

        public bool EsPrincipal { get; set; }

        public DateTime? UltimaActualizacionPrecio { get; set; }

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}