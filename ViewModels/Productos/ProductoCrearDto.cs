using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels.Productos
{
    /// <summary>
    /// DTO para crear un nuevo producto
    /// </summary>
    public class ProductoCrearDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }
        
        [StringLength(20, ErrorMessage = "El nombre corto no puede exceder los 20 caracteres")]
        public string? NombreCortoTPV { get; set; }
        
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }
        
        [StringLength(50, ErrorMessage = "El PLU no puede exceder los 50 caracteres")]
        public string? PLU { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor o igual a 0")]
        public decimal PrecioVenta { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
        public decimal Costo { get; set; }
        
        public string? ImagenUrl { get; set; }
        
        [StringLength(7, ErrorMessage = "El color debe ser un código hexadecimal de 7 caracteres")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "El color debe ser un código hexadecimal válido (ej: #FF0000)")]
        public string? ColorBotonTPV { get; set; }
        
        public int OrdenClasificacion { get; set; } = 0;
        
        public bool EsActivo { get; set; } = true;
        
        public bool PermiteModificadores { get; set; } = true;
        
        public bool RequierePuntoCoccion { get; set; } = false;
        
        // Relaciones
        public int? ItemId { get; set; }
        
        public int? ItemContenedorId { get; set; }
        
        [Required(ErrorMessage = "La categoría es requerida")]
        public int CategoriaId { get; set; }
        
        // Mantener por compatibilidad
        [Obsolete("Use ImpuestoIds instead")]
        public int? ImpuestoId { get; set; }
        
        // Nueva lista de impuestos
        public List<int> ImpuestoIds { get; set; } = new List<int>();
        
        public int? RutaImpresoraId { get; set; }
        
        // Campos adicionales
        public decimal Cantidad { get; set; } = 1;
        
        public bool DisponibleParaVenta { get; set; } = true;
        
        public bool RequierePreparacion { get; set; } = false;
        
        public int? TiempoPreparacion { get; set; }
        
        public int EmpresaId { get; set; }
        
        // Propiedades de Contabilidad
        [Display(Name = "Cuenta de Ventas")]
        public int? CuentaVentasId { get; set; }
        
        [Display(Name = "Cuenta de Compras/Inventarios")]
        public int? CuentaComprasInventariosId { get; set; }
        
        [Display(Name = "Cuenta de Costo de Ventas")]
        public int? CuentaCostoVentasGastosId { get; set; }
        
        [Display(Name = "Cuenta de Descuentos")]
        public int? CuentaDescuentosId { get; set; }
        
        [Display(Name = "Cuenta de Devoluciones")]
        public int? CuentaDevolucionesId { get; set; }
        
        [Display(Name = "Cuenta de Ajustes")]
        public int? CuentaAjustesId { get; set; }
        
        [Display(Name = "Cuenta de Costo de Materia Prima")]
        public int? CuentaCostoMateriaPrimaId { get; set; }
    }
}