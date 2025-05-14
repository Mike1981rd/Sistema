using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models.ViewModels
{
    public class CompraViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número es requerido")]
        [StringLength(20, ErrorMessage = "El número no puede exceder los 20 caracteres")]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El proveedor es requerido")]
        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }
        public string ProveedorNombre { get; set; }

        [Display(Name = "Almacén")]
        public int? AlmacenId { get; set; }
        public string AlmacenNombre { get; set; }

        [StringLength(50, ErrorMessage = "La referencia no puede exceder los 50 caracteres")]
        [Display(Name = "Referencia")]
        public string Referencia { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [Display(Name = "Plazo de Pago")]
        public int? PlazoPagoId { get; set; }
        public string PlazoPagoNombre { get; set; }

        [Display(Name = "Fecha de Vencimiento")]
        [DataType(DataType.Date)]
        public DateTime? FechaVencimiento { get; set; }

        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Descuento")]
        public decimal Descuento { get; set; }

        [Display(Name = "Impuestos")]
        public decimal Impuestos { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Pendiente";

        public int? EntradaDiarioId { get; set; }

        public int EmpresaId { get; set; }

        public List<CompraDetalleViewModel> Detalles { get; set; } = new List<CompraDetalleViewModel>();

        // Propiedades para SelectLists
        public SelectList Proveedores { get; set; }
        public SelectList Almacenes { get; set; }
        public SelectList PlazosPago { get; set; }
        public SelectList ImpuestosSelectList { get; set; }
        public SelectList UnidadesMedida { get; set; }
    }

    public class CompraDetalleViewModel
    {
        public int Id { get; set; }
        public int CompraId { get; set; }

        [Required(ErrorMessage = "El producto es requerido")]
        public int ItemId { get; set; }
        public string ItemNombre { get; set; }
        public string ItemCodigo { get; set; }

        [StringLength(150, ErrorMessage = "La descripción no puede exceder los 150 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        public decimal Precio { get; set; }
        
        public decimal Subtotal { get; set; }
        
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        public decimal PorcentajeDescuento { get; set; }
        
        public decimal MontoDescuento { get; set; }
        
        public int? ImpuestoId { get; set; }
        public string ImpuestoNombre { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
        
        public decimal MontoImpuesto { get; set; }
        
        public decimal Total { get; set; }
        
        [Required(ErrorMessage = "La unidad de medida es requerida")]
        public int UnidadMedidaId { get; set; }
        public string UnidadMedidaNombre { get; set; }
        
        public decimal FactorConversion { get; set; } = 1;
        
        public int EmpresaId { get; set; }
    }
} 