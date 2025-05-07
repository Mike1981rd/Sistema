using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class ConciliacionBancaria
    {
        public int Id { get; set; }
        
        [Required]
        public int BancoId { get; set; }
        public virtual Banco? Banco { get; set; }
        
        [Required]
        public DateTime FechaInicio { get; set; }
        
        [Required]
        public DateTime FechaFin { get; set; }
        
        [Required]
        public DateTime FechaConciliacion { get; set; } = DateTime.Now;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoSegunLibro { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoSegunBanco { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiferenciaConciliacion { get; set; }
        
        [StringLength(500)]
        public string? Notas { get; set; }
        
        public EstadoConciliacion Estado { get; set; } = EstadoConciliacion.Borrador;
        
        // Relación con las transacciones bancarias
        public virtual ICollection<TransaccionBanco>? TransaccionesConciliadas { get; set; }
        
        // Relación con los ajustes de conciliación
        public virtual ICollection<AjusteConciliacion>? Ajustes { get; set; }
        
        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
    
    public enum EstadoConciliacion
    {
        Borrador,
        Pendiente,
        Aprobado,
        Rechazado
    }
    
    public class AjusteConciliacion
    {
        public int Id { get; set; }
        
        [Required]
        public int ConciliacionId { get; set; }
        public virtual ConciliacionBancaria? Conciliacion { get; set; }
        
        [Required]
        public TipoAjusteConciliacion Tipo { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Concepto { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Descripcion { get; set; }
        
        public bool Aplicado { get; set; } = false;
        
        // Relación con asiento contable generado
        public int? AsientoContableId { get; set; }
        public virtual AsientoContable? AsientoContable { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public string? UsuarioCreacion { get; set; }
    }
    
    public enum TipoAjusteConciliacion
    {
        DepositoNoRegistrado,
        RetiroNoRegistrado,
        ComisionBancaria,
        ErrorContabilidad,
        Otro
    }
} 