using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SistemaContable.Models
{
    public class Familia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Nota { get; set; }

        [Required]
        public bool Estado { get; set; } = true; // true = Activo, false = Inactivo

        // Cuentas Contables específicas
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

        // --- CAMPOS REQUERIDOS POR LA BASE DE DATOS ---
        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        
        #if DEBUG
        // Esta línea se mantendrá solo para debugging mientras se completa la transición
        [Obsolete("Esta propiedad está obsoleta y será eliminada en una futura versión.")]
        public virtual ICollection<FamiliaCuentaContable>? FamiliaCuentasContables { get; set; }
        #endif
    }
}
