using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels.Productos
{
    /// <summary>
    /// DTO para mostrar el detalle completo de un producto
    /// </summary>
    public class ProductoDetalleDto
    {
        public int Id { get; set; }
        
        public string Nombre { get; set; }
        
        public string? NombreCortoTPV { get; set; }
        
        public string? Descripcion { get; set; }
        
        public string? PLU { get; set; }
        
        public decimal PrecioVenta { get; set; }
        
        public decimal Costo { get; set; }
        
        public string? ImagenUrl { get; set; }
        
        public string? ColorBotonTPV { get; set; }
        
        public int OrdenClasificacion { get; set; }
        
        public bool EsActivo { get; set; }
        
        public bool PermiteModificadores { get; set; }
        
        public bool RequierePuntoCoccion { get; set; }
        
        public decimal Cantidad { get; set; }
        
        public decimal CostoTotal { get; set; }
        
        public bool DisponibleParaVenta { get; set; }
        
        public bool RequierePreparacion { get; set; }
        
        public int? TiempoPreparacion { get; set; }
        
        // Relaciones
        public int? ItemId { get; set; }
        
        public int? ItemContenedorId { get; set; }
        
        // Categoria
        public int CategoriaId { get; set; }
        public CategoriaSimpleDto Categoria { get; set; }
        
        // Impuesto (mantener por compatibilidad)
        [Obsolete("Use Impuestos collection instead")]
        public int? ImpuestoId { get; set; }
        [Obsolete("Use Impuestos collection instead")]
        public ImpuestoSimpleDto? Impuesto { get; set; }
        
        // Nueva colección de impuestos
        public List<ImpuestoSimpleDto> Impuestos { get; set; } = new List<ImpuestoSimpleDto>();
        
        // Ruta de Impresión
        public int? RutaImpresoraId { get; set; }
        public RutaImpresoraSimpleDto? RutaImpresora { get; set; }
        
        // Colecciones
        public List<VarianteProductoDto> Variantes { get; set; } = new List<VarianteProductoDto>();
        
        public List<GrupoModificadoresAsignadoDto> GruposModificadores { get; set; } = new List<GrupoModificadoresAsignadoDto>();
        
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
        
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
    
    /// <summary>
    /// DTO simplificado para Categoría
    /// </summary>
    public class CategoriaSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    
    /// <summary>
    /// DTO simplificado para Impuesto
    /// </summary>
    public class ImpuestoSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal? Porcentaje { get; set; }
    }
    
    /// <summary>
    /// DTO simplificado para Ruta de Impresora
    /// </summary>
    public class RutaImpresoraSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    
    /// <summary>
    /// DTO para Variante de Producto
    /// </summary>
    public class VarianteProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? PLUVariante { get; set; }
        public decimal PrecioAdicionalOAbsoluto { get; set; }
        public string AjustePrecioTipo { get; set; }
        public int? Stock { get; set; }
        public int OrdenClasificacion { get; set; }
        public bool EsActivo { get; set; }
    }
    
    /// <summary>
    /// DTO para Grupo de Modificadores asignado a un producto
    /// </summary>
    public class GrupoModificadoresAsignadoDto
    {
        public int GrupoModificadoresId { get; set; }
        public string Nombre { get; set; }
        public bool EsForzado { get; set; }
        public int MinSeleccion { get; set; }
        public int MaxSeleccion { get; set; }
        public string TipoVisualizacionTPV { get; set; }
        public int OrdenEspecificoProducto { get; set; }
        public List<ModificadorSimpleDto> Modificadores { get; set; } = new List<ModificadorSimpleDto>();
    }
    
    /// <summary>
    /// DTO simplificado para Modificador
    /// </summary>
    public class ModificadorSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioAdicional { get; set; }
        public int OrdenClasificacion { get; set; }
        public bool EsActivo { get; set; }
    }
}