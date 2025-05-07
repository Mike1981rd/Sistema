using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class AsientoContable
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Numero { get; set; } = string.Empty;
        
        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;
        
        [Required]
        [StringLength(200)]
        public string Concepto { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Descripcion { get; set; }
        
        public bool Contabilizado { get; set; } = false;
        
        public DateTime? FechaContabilizacion { get; set; }
        
        public TipoAsientoContable Tipo { get; set; } = TipoAsientoContable.Normal;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoTotal { get; set; } = 0;
        
        // Origen del asiento
        public string? OrigenDocumento { get; set; }
        public int? OrigenId { get; set; }
        
        // Relación con los detalles del asiento
        public virtual ICollection<DetalleAsientoContable>? Detalles { get; set; }
        
        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
    
    public class DetalleAsientoContable
    {
        public int Id { get; set; }
        
        [Required]
        public int AsientoContableId { get; set; }
        public virtual AsientoContable? AsientoContable { get; set; }
        
        [Required]
        public int CuentaContableId { get; set; }
        public virtual CuentaContable? CuentaContable { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debe { get; set; } = 0;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Haber { get; set; } = 0;
        
        [StringLength(200)]
        public string? Concepto { get; set; }
        
        // Opcional, para asociar el movimiento a un tercero
        public int? ContactoId { get; set; }
        public virtual Contacto? Contacto { get; set; }
    }
    
    public enum TipoAsientoContable
    {
        Normal,
        Apertura,
        Ajuste,
        Cierre
    }
} 