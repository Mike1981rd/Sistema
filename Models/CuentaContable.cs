using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class CuentaContable
    {
        public ICollection<FamiliaCuentaContable> FamiliaCuentasContables { get; set; }
        
        public int Id { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Descripcion { get; set; }
        
        [Required]
        public string Categoria { get; set; } = "Activo"; // Activo, Pasivo, Patrimonio, Ingreso, Gasto, Costo, CuentaOrden
        
        [Required]
        public string Naturaleza { get; set; } = "Deudora"; // Deudora o Acreedora
        
        [Required]
        public string TipoCuenta { get; set; } = "Mayor"; // Mayor o Movimiento
        
        public string? UsoCuenta { get; set; } // CuentasPorCobrar, CuentasPorPagar, Bancos, Inventario, Ventas, etc.
        
        public bool VerSaldoPorTercero { get; set; }
        
        public int? CuentaPadreId { get; set; }
        public virtual CuentaContable? CuentaPadre { get; set; }
        
        public virtual ICollection<CuentaContable>? SubCuentas { get; set; }
        
        public int Nivel { get; set; }
        
        public int Orden { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public bool EsCuentaSistema { get; set; }
        
        [NotMapped]
        public bool TieneDependencias => SubCuentas?.Count > 0;
        
        [NotMapped]
        public string CodigoNombre => $"{Codigo} - {Nombre}";
        
        // ID de la empresa a la que pertenece esta cuenta
        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
    }
} 