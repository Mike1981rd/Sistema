using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class TransaccionBanco
    {
        public int Id { get; set; }
        
        [Required]
        public int BancoId { get; set; }
        public virtual Banco? Banco { get; set; }
        
        [Required]
        public TipoTransaccionBancaria Tipo { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }
        
        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;
        
        [Required]
        [StringLength(200)]
        public string Concepto { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Referencia { get; set; }
        
        [StringLength(500)]
        public string? Descripcion { get; set; }
        
        public bool Conciliado { get; set; } = false;
        
        public int? ConciliacionId { get; set; }
        public virtual ConciliacionBancaria? Conciliacion { get; set; }
        
        // Relación con contacto (beneficiario/pagador)
        public int? ContactoId { get; set; }
        public virtual Contacto? Contacto { get; set; }
        
        // Relación con asiento contable generado
        public int? AsientoContableId { get; set; }
        public virtual AsientoContable? AsientoContable { get; set; }
        
        // Para transferencias entre cuentas
        public int? BancoDestinoId { get; set; }
        public virtual Banco? BancoDestino { get; set; }
        
        // Nueva propiedad para transferencias a cuentas contables
        public int? CuentaContableDestinoId { get; set; }
        public virtual CuentaContable? CuentaContableDestino { get; set; }
        
        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        
        [NotMapped]
        public string TipoFormatted => Tipo.ToString();
        
        [NotMapped]
        public bool EsIngreso => 
            Tipo == TipoTransaccionBancaria.Deposito || 
            Tipo == TipoTransaccionBancaria.CobroCliente || 
            Tipo == TipoTransaccionBancaria.Ingreso;
            
        [NotMapped]
        public bool EsEgreso => 
            Tipo == TipoTransaccionBancaria.Retiro || 
            Tipo == TipoTransaccionBancaria.PagoProveedor || 
            Tipo == TipoTransaccionBancaria.GastoBancario || 
            Tipo == TipoTransaccionBancaria.Cheque;
    }
    
    public enum TipoTransaccionBancaria
    {
        Ingreso,
        Gasto,
        Transferencia,
        Ajuste,
        Deposito,
        Retiro,
        PagoProveedor,
        CobroCliente,
        GastoBancario,
        Cheque
    }
} 