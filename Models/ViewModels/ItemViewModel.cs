using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.Models.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(50, ErrorMessage = "El código no puede exceder los 50 caracteres")]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [StringLength(50)]
        [Display(Name = "Código de Barras")]
        public string? CodigoBarras { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(500)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;

        [Display(Name = "Imagen")]
        public string? ImagenUrl { get; set; }

        // Campo para la carga de imagen
        [Display(Name = "Imagen del Item")]
        public IFormFile? ItemImage { get; set; }

        // Primera pestaña - Información básica
        [Required(ErrorMessage = "La categoría es requerida")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }
        public SelectList? CategoriasDisponibles { get; set; }

        [Display(Name = "Marca")]
        public int? MarcaId { get; set; }
        public SelectList? MarcasDisponibles { get; set; }

        [Display(Name = "Impuesto")]
        public int? ImpuestoId { get; set; }
        public SelectList? ImpuestosDisponibles { get; set; }

        [Display(Name = "Rendimiento (%)")]
        [Range(0, 100, ErrorMessage = "El rendimiento debe estar entre 0 y 100")]
        public decimal Rendimiento { get; set; } = 100;

        // Segunda pestaña - Compras
        [Required(ErrorMessage = "La unidad de medida de inventario es requerida")]
        [Display(Name = "Unidad de Medida (Inventario)")]
        public int UnidadMedidaInventarioId { get; set; }
        public SelectList? UnidadesMedidaDisponibles { get; set; }

        [Display(Name = "Nivel Mínimo")]
        [Range(0, double.MaxValue, ErrorMessage = "El nivel mínimo debe ser mayor o igual a 0")]
        public decimal NivelMinimo { get; set; }

        [Display(Name = "Stock Actual")]
        [Range(0, double.MaxValue, ErrorMessage = "El stock actual debe ser mayor o igual a 0")]
        public decimal StockActual { get; set; }

        // Tercera pestaña - Contabilidad
        [Display(Name = "Cuenta de Ventas")]
        public int? CuentaVentasId { get; set; }
        public SelectList? CuentasVentasDisponibles { get; set; }

        [Display(Name = "Cuenta de Compras/Inventarios")]
        public int? CuentaComprasInventariosId { get; set; }
        public SelectList? CuentasComprasInventariosDisponibles { get; set; }

        [Display(Name = "Cuenta de Costo de Ventas")]
        public int? CuentaCostoVentasGastosId { get; set; }
        public SelectList? CuentasCostoVentasGastosDisponibles { get; set; }

        [Display(Name = "Cuenta de Descuentos")]
        public int? CuentaDescuentosId { get; set; }
        public SelectList? CuentasDescuentosDisponibles { get; set; }

        [Display(Name = "Cuenta de Devoluciones")]
        public int? CuentaDevolucionesId { get; set; }
        public SelectList? CuentasDevolucionesDisponibles { get; set; }

        [Display(Name = "Cuenta de Ajustes")]
        public int? CuentaAjustesId { get; set; }
        public SelectList? CuentasAjustesDisponibles { get; set; }

        // Colecciones para las relaciones
        public List<ItemProveedorViewModel> Proveedores { get; set; } = new();
        public List<ItemContenedorViewModel> Contenedores { get; set; } = new();
        public List<ItemTaraViewModel> Taras { get; set; } = new();
        public List<ItemAlmacenViewModel> Almacenes { get; set; } = new();
        public ProductoVentaViewModel? ProductoVenta { get; set; }

        // Lista de proveedores disponibles
        public SelectList? ProveedoresDisponibles { get; set; }

        // Para el mapeo con Empresa
        public int EmpresaId { get; set; }
        
        // Para información de separador decimal
        public string? SeparadorDecimal { get; set; }
    }
} 