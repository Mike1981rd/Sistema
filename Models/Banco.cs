using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class Banco
    {
        public Banco()
        {
            Transacciones = new HashSet<TransaccionBanco>();
            Conciliaciones = new HashSet<ConciliacionBancaria>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NumeroCuenta { get; set; } = string.Empty;

        [Required]
        public TipoCuentaBancaria TipoCuenta { get; set; } = TipoCuentaBancaria.Corriente;

        [Required]
        [StringLength(100)]
        public string EntidadBancaria { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Moneda { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoInicial { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoActual { get; set; } = 0;

        [NotMapped]
        public decimal SaldoPendienteReconciliacion => SaldoActual - SaldoConciliado;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoConciliado { get; set; } = 0;

        public DateTime FechaApertura { get; set; } = DateTime.Now;

        public bool Activo { get; set; } = true;

        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }

        public int CuentaContableId { get; set; }
        public virtual CuentaContable? CuentaContable { get; set; }

        // Relaciones con otras entidades
        public virtual ICollection<TransaccionBanco>? Transacciones { get; set; }
        public virtual ICollection<ConciliacionBancaria>? Conciliaciones { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }

        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }

        // Propiedades calculadas
        [NotMapped]
        public decimal PendienteConciliar
        {
            get
            {
                decimal pendiente = 0;
                
                if (Transacciones != null)
                {
                    foreach (var transaccion in Transacciones.Where(t => !t.Conciliado))
                    {
                        if (transaccion.Tipo == TipoTransaccionBancaria.Ingreso || 
                            transaccion.Tipo == TipoTransaccionBancaria.Ajuste && transaccion.Monto > 0)
                        {
                            pendiente += transaccion.Monto;
                        }
                        else if (transaccion.Tipo == TipoTransaccionBancaria.Gasto || 
                                transaccion.Tipo == TipoTransaccionBancaria.Ajuste && transaccion.Monto < 0)
                        {
                            pendiente -= Math.Abs(transaccion.Monto);
                        }
                    }
                }
                
                return pendiente;
            }
        }

        [NotMapped]
        [Display(Name = "Última conciliación")]
        public DateTime? UltimaConciliacion
        {
            get
            {
                if (Conciliaciones != null && Conciliaciones.Any())
                {
                    return Conciliaciones.Max(c => c.FechaConciliacion);
                }
                return null;
            }
        }
    }

    public enum TipoCuentaBancaria
    {
        Corriente,
        Ahorro,
        Inversion,
        Credito,
        Otro
    }
} 