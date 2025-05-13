using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace SistemaContable.Models
{
    public class Item : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(50, ErrorMessage = "El código no puede exceder los 50 caracteres")]
        public string Codigo { get; set; }

        [StringLength(50)]
        public string? CodigoBarras { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public bool Estado { get; set; } = true;

        [StringLength(255)]
        public string? ImagenUrl { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }

        public int? MarcaId { get; set; }
        [ForeignKey("MarcaId")]
        public virtual Marca? Marca { get; set; }

        [Required(ErrorMessage = "La unidad de medida es requerida")]
        public int UnidadMedidaInventarioId { get; set; }
        [ForeignKey("UnidadMedidaInventarioId")]
        public virtual UnidadMedida? UnidadMedidaInventario { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Rendimiento { get; set; } = 100;

        [Column(TypeName = "decimal(18,4)")]
        public decimal NivelMinimo { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal StockActual { get; set; }

        public int? ImpuestoId { get; set; }
        [ForeignKey("ImpuestoId")]
        public virtual Impuesto? Impuesto { get; set; }

        // Cuentas contables
        public int? CuentaVentasId { get; set; }
        [ForeignKey("CuentaVentasId")]
        public virtual CuentaContable? CuentaVentas { get; set; }

        public int? CuentaComprasInventariosId { get; set; }
        [ForeignKey("CuentaComprasInventariosId")]
        public virtual CuentaContable? CuentaComprasInventarios { get; set; }

        public int? CuentaCostoVentasGastosId { get; set; }
        [ForeignKey("CuentaCostoVentasGastosId")]
        public virtual CuentaContable? CuentaCostoVentasGastos { get; set; }

        public int? CuentaDescuentosId { get; set; }
        [ForeignKey("CuentaDescuentosId")]
        public virtual CuentaContable? CuentaDescuentos { get; set; }

        public int? CuentaDevolucionesId { get; set; }
        [ForeignKey("CuentaDevolucionesId")]
        public virtual CuentaContable? CuentaDevoluciones { get; set; }

        public int? CuentaAjustesId { get; set; }
        [ForeignKey("CuentaAjustesId")]
        public virtual CuentaContable? CuentaAjustes { get; set; }

        // Relaciones de navegación
        public virtual ICollection<ItemProveedor>? Proveedores { get; set; }
        public virtual ICollection<ItemAlmacen>? Almacenes { get; set; }
        public virtual ICollection<ItemContenedor>? Contenedores { get; set; }
        public virtual ICollection<ItemTara>? Taras { get; set; }
        public virtual ICollection<ProductoVenta>? ProductosVenta { get; set; }

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}