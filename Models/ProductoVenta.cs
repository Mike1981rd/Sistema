using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class ProductoVenta : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        
        [StringLength(20)]
        public string? NombreCortoTPV { get; set; }
        
        [StringLength(500)]
        public string? Descripcion { get; set; }
        
        [StringLength(50)]
        public string? PLU { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioVenta { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Costo { get; set; }
        
        public string? ImagenUrl { get; set; }
        
        [StringLength(7)]
        public string? ColorBotonTPV { get; set; }
        
        public int OrdenClasificacion { get; set; } = 0;
        
        public bool EsActivo { get; set; } = true;
        
        public bool PermiteModificadores { get; set; } = true;
        
        public bool RequierePuntoCoccion { get; set; } = false;
        
        // Relaciones existentes
        public int? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        public int? ItemContenedorId { get; set; }
        [ForeignKey("ItemContenedorId")]
        public virtual ItemContenedor? ItemContenedor { get; set; }

        // Relaciones de categor�a e impuesto
        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }
        
        // Mantener por compatibilidad pero marcar como obsoleto
        [Obsolete("Use ProductoVentaImpuestos collection instead. This field will be removed in future versions.")]
        public int? ImpuestoId { get; set; }
        [ForeignKey("ImpuestoId")]
        [Obsolete("Use ProductoVentaImpuestos collection instead. This field will be removed in future versions.")]
        public virtual Impuesto? Impuesto { get; set; }
        
        // Nueva relación muchos-a-muchos con Impuestos
        public virtual ICollection<ProductoVentaImpuesto>? ProductoVentaImpuestos { get; set; }
        
        public int? RutaImpresoraId { get; set; }
        [ForeignKey("RutaImpresoraId")]
        public virtual RutaImpresora? RutaImpresora { get; set; }

        // Nuevas colecciones de navegaci�n
        public virtual ICollection<VarianteProducto>? Variantes { get; set; }
        public virtual ICollection<ProductoModificadorGrupo>? ProductoModificadorGrupos { get; set; }
        public virtual ICollection<RecetaIngrediente>? IngredientesDeEsteProducto { get; set; }
        public virtual ICollection<RecetaIngrediente>? ApareceComoIngredienteEn { get; set; }
        public virtual ICollection<PaqueteComponente>? ComponentesDeEstePaquete { get; set; }
        public virtual ICollection<PaqueteComponente>? ApareceComoComponenteEn { get; set; }
        
        // Nueva relaci�n para m�ltiples niveles de precio
        public virtual ICollection<ProductoVentaPrecio>? NivelesPrecios { get; set; }

        // Campos adicionales para compatibilidad
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cantidad { get; set; } = 1;
        
        // Calculado: Costo * Cantidad
        [Column(TypeName = "decimal(18,4)")]
        public decimal CostoTotal { get; set; }
        
        // Indica si este producto está disponible para venta
        public bool DisponibleParaVenta { get; set; } = true;
        
        // Indica si requiere preparación (útil para restaurantes)
        public bool RequierePreparacion { get; set; }
        
        // Tiempo estimado de preparación (en minutos)
        public int? TiempoPreparacion { get; set; }
        
        // Auditor�a
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
        
        // Propiedades de Contabilidad
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
        
        public int? CuentaCostoMateriaPrimaId { get; set; }
        [ForeignKey("CuentaCostoMateriaPrimaId")]
        public virtual CuentaContable? CuentaCostoMateriaPrima { get; set; }
        
        // Campos para Recetas
        [StringLength(1000)]
        public string? NotasReceta { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? MargenGananciaReceta { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? CostoTotalReceta { get; set; }
        
        // Métodos helper para trabajar con múltiples precios
        
        /// <summary>
        /// Obtiene el precio principal del producto
        /// </summary>
        public ProductoVentaPrecio? ObtenerPrecioPrincipal()
        {
            return NivelesPrecios?
                .Where(p => p.Activo && p.EsPrincipal)
                .OrderBy(p => p.Orden)
                .FirstOrDefault();
        }
        
        /// <summary>
        /// Obtiene todos los precios activos ordenados por orden
        /// </summary>
        public IEnumerable<ProductoVentaPrecio> ObtenerPreciosActivos()
        {
            return NivelesPrecios?
                .Where(p => p.Activo)
                .OrderBy(p => p.Orden)
                .ThenBy(p => p.Id) ?? Enumerable.Empty<ProductoVentaPrecio>();
        }
        
        /// <summary>
        /// Obtiene el precio efectivo para mostrar (precio principal o PrecioVenta para compatibilidad)
        /// </summary>
        public decimal ObtenerPrecioEfectivo()
        {
            var precioPrincipal = ObtenerPrecioPrincipal();
            return precioPrincipal?.PrecioTotal ?? PrecioVenta;
        }
        
        /// <summary>
        /// Verifica si el producto tiene múltiples niveles de precio configurados
        /// </summary>
        public bool TieneMultiplesPrecios()
        {
            return NivelesPrecios?.Count(p => p.Activo) > 1;
        }
    }
}