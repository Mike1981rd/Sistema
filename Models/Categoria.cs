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

        // Impuesto
        public int? ImpuestoId { get; set; }
        [ForeignKey("ImpuestoId")]
        public virtual Impuesto? Impuesto { get; set; }

        // Usuario que creó la categoría
        public int? UsuarioCreacionId { get; set; }

        // Ruta de impresora
        public int? RutaImpresoraId { get; set; }
        [ForeignKey("RutaImpresoraId")]
        public virtual RutaImpresora? RutaImpresora { get; set; }

        // Campos para impuestos
        public string? Impuestos { get; set; }
        
        // Propina
        public decimal? Propina { get; set; }

        /// <summary>
        /// Obsoleta: Use RutaImpresoraId en su lugar
        /// </summary>
        [Obsolete("Use RutaImpresoraId instead", true)]
        public string? CanalesImpresora 
        { 
            get => RutaImpresoraId?.ToString(); 
            set 
            {
                if (int.TryParse(value, out int id))
                    RutaImpresoraId = id;
            }
        }

        // Campos de auditoría
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }

        [Required]
        public int EmpresaId { get; set; }
        [ForeignKey("EmpresaId")]
        public virtual Empresa? Empresa { get; set; }

        public int? PropinaImpuestoId { get; set; }
        [ForeignKey("PropinaImpuestoId")]
        public virtual Impuesto? PropinaImpuesto { get; set; }
    }
} 