using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class SaldoInicial
    {
        public int Id { get; set; }
        
        [Required]
        public int CuentaContableId { get; set; }
        public virtual CuentaContable? CuentaContable { get; set; }
        
        // Opcional, para saldos por tercero
        public int? ContactoId { get; set; }
        public virtual Contacto? Contacto { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; } = 0;
        
        [Required]
        public DateTime FechaInicial { get; set; } = DateTime.UtcNow;
        
        // ID de la empresa 
        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }
    }
} 