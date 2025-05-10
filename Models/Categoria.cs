using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class Categoria
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

        // Relación con Familia (Grupo en la UI)
        [Required]
        public int FamiliaId { get; set; }
        public virtual Familia? Familia { get; set; }

        // Cuentas Contables específicas (heredadas de Familia pero pueden sobreescribirse)
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

        // Campos para impuestos
        public string? Impuestos { get; set; }
        
        // Propina
        public decimal? Propina { get; set; }
        
        // Canales de impresora
        public string? CanalesImpresora { get; set; }

        // Campos requeridos por la base de datos
        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }

        public int? PropinaImpuestoId { get; set; }
        [ForeignKey("PropinaImpuestoId")]
        public virtual Impuesto? PropinaImpuesto { get; set; }
    }
} 