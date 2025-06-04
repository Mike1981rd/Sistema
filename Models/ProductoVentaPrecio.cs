using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    /// <summary>
    /// Tabla para manejar múltiples niveles de precio por producto
    /// Permite que un producto tenga varios precios (base, mayorista, etc.)
    /// </summary>
    public class ProductoVentaPrecio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoVentaId { get; set; }
        [ForeignKey("ProductoVentaId")]
        public virtual ProductoVenta ProductoVenta { get; set; } = null!;

        /// <summary>
        /// Nombre descriptivo del nivel de precio (ej: "Precio Base", "Precio Mayorista")
        /// </summary>
        [Required]
        [StringLength(100)]
        public string NombreNivel { get; set; } = string.Empty;

        /// <summary>
        /// Precio base sin impuestos
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal PrecioBase { get; set; }

        /// <summary>
        /// Precio total con impuestos incluidos
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal PrecioTotal { get; set; }

        /// <summary>
        /// Orden de visualización/prioridad
        /// </summary>
        public int Orden { get; set; } = 0;

        /// <summary>
        /// Indica si este es el precio principal del producto
        /// </summary>
        public bool EsPrincipal { get; set; } = false;

        /// <summary>
        /// Referencia opcional a ListaPrecio para integración futura
        /// </summary>
        public int? ListaPrecioId { get; set; }
        [ForeignKey("ListaPrecioId")]
        public virtual ListaPrecio? ListaPrecio { get; set; }

        /// <summary>
        /// Descripción opcional del nivel de precio
        /// </summary>
        [StringLength(255)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Estado activo/inactivo
        /// </summary>
        public bool Activo { get; set; } = true;

        // Campos de auditoría 
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }
        
        /// <summary>
        /// ID de la empresa (multi-empresa)
        /// </summary>
        [Required]
        public int EmpresaId { get; set; }

        /// <summary>
        /// Impuestos específicos de este nivel de precio (opcional)
        /// Si está vacío, hereda los impuestos del producto
        /// </summary>
        public virtual ICollection<ProductoVentaPrecioImpuesto>? ImpuestosEspecificos { get; set; }

        /// <summary>
        /// Método helper para obtener los impuestos efectivos de este precio
        /// </summary>
        public IEnumerable<Impuesto> ObtenerImpuestosEfectivos()
        {
            // Si tiene impuestos específicos, usar esos
            if (ImpuestosEspecificos?.Any() == true)
            {
                return ImpuestosEspecificos
                    .Where(pi => pi.Impuesto != null)
                    .OrderBy(pi => pi.Orden)
                    .Select(pi => pi.Impuesto!);
            }
            
            // Si no, usar los impuestos por defecto del producto
            return ProductoVenta?.ProductoVentaImpuestos?
                .Where(pi => pi.Impuesto != null)
                .OrderBy(pi => pi.Orden)
                .Select(pi => pi.Impuesto!) ?? Enumerable.Empty<Impuesto>();
        }

        /// <summary>
        /// Recalcula el precio total basado en el precio base y los impuestos efectivos
        /// </summary>
        public void RecalcularPrecioTotal()
        {
            var impuestos = ObtenerImpuestosEfectivos();
            var totalImpuestosPorcentaje = impuestos.Sum(i => (i.Porcentaje ?? 0) / 100m);
            PrecioTotal = PrecioBase * (1 + totalImpuestosPorcentaje);
        }

        /// <summary>
        /// Recalcula el precio base basado en el precio total y los impuestos efectivos
        /// </summary>
        public void RecalcularPrecioBase()
        {
            var impuestos = ObtenerImpuestosEfectivos();
            var totalImpuestosPorcentaje = impuestos.Sum(i => (i.Porcentaje ?? 0) / 100m);
            
            if ((1 + totalImpuestosPorcentaje) > 0)
            {
                PrecioBase = PrecioTotal / (1 + totalImpuestosPorcentaje);
            }
            else
            {
                PrecioBase = PrecioTotal;
            }
        }
    }
}