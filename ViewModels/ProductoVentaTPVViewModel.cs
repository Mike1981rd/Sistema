using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaContable.Models;
using SistemaContable.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels
{
    public class ProductoVentaTPVViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(20)]
        [Display(Name = "Nombre Corto TPV")]
        public string? NombreCortoTPV { get; set; }

        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [StringLength(50)]
        [Display(Name = "PLU")]
        public string? PLU { get; set; }

        [Required(ErrorMessage = "El precio de venta es requerido")]
        [Display(Name = "Precio de Venta")]
        [DataType(DataType.Currency)]
        public decimal PrecioVenta { get; set; }

        [Display(Name = "Costo")]
        [DataType(DataType.Currency)]
        public decimal Costo { get; set; } = 0;

        [Display(Name = "Imagen")]
        public IFormFile? ImagenFile { get; set; }
        public string? ImagenUrl { get; set; }

        [Display(Name = "Color del Botón (Hex)")]
        [StringLength(7)]
        [RegularExpression(@"^#([A-Fa-f0-9]{6})$", ErrorMessage = "Debe ser un código hexadecimal válido (#RRGGBB)")]
        public string? ColorBotonTPV { get; set; }

        [Display(Name = "Orden de Clasificación")]
        public int OrdenClasificacion { get; set; } = 0;

        [Display(Name = "Activo")]
        public bool EsActivo { get; set; } = true;

        [Display(Name = "Permite Modificadores")]
        public bool PermiteModificadores { get; set; } = true;

        [Display(Name = "Requiere Punto de Cocción")]
        public bool RequierePuntoCoccion { get; set; } = false;

        // Relaciones
        [Required(ErrorMessage = "La categoría es requerida")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [Display(Name = "Impuesto")]
        public int? ImpuestoId { get; set; }

        [Display(Name = "Ruta de Impresión")]
        public int? RutaImpresoraId { get; set; }

        // Listas para dropdowns
        public SelectList? CategoriasDisponibles { get; set; }
        public SelectList? ImpuestosDisponibles { get; set; }
        public SelectList? RutasImpresoraDisponibles { get; set; }

        // Colecciones relacionadas
        public List<VarianteProductoViewModel> Variantes { get; set; } = new();
        public List<ProductoModificadorGrupoViewModel> GruposModificadores { get; set; } = new();
        public List<RecetaIngredienteViewModel> Ingredientes { get; set; } = new();
        public List<PaqueteComponenteViewModel> ComponentesPaquete { get; set; } = new();

        // Para migración desde Items existentes
        public int? ItemId { get; set; }
        public bool MigrarDesdeItem { get; set; } = false;
        
        // Campos adicionales para compatibilidad
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; } = 1;
        
        [Display(Name = "Costo Total")]
        public decimal CostoTotal => Costo * Cantidad;
        
        [Display(Name = "Disponible para Venta")]
        public bool DisponibleParaVenta { get; set; } = true;
        
        [Display(Name = "Requiere Preparación")]
        public bool RequierePreparacion { get; set; }
        
        [Display(Name = "Tiempo de Preparación (minutos)")]
        public int? TiempoPreparacion { get; set; }
    }
}