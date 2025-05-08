using System;
using System.ComponentModel.DataAnnotations;
using SistemaContable.Models;
using Microsoft.AspNetCore.Http;

namespace SistemaContable.ViewModels
{
    public class BancoViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre de la cuenta")]
        public string Nombre { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El número de cuenta es obligatorio")]
        [StringLength(100, ErrorMessage = "El número de cuenta no puede exceder 100 caracteres")]
        [Display(Name = "Número de cuenta")]
        public string NumeroCuenta { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El tipo de cuenta es obligatorio")]
        [Display(Name = "Tipo de cuenta")]
        public TipoCuentaBancaria TipoCuenta { get; set; } = TipoCuentaBancaria.Corriente;
        
        [Required(ErrorMessage = "La entidad bancaria es obligatoria")]
        [StringLength(100, ErrorMessage = "La entidad bancaria no puede exceder 100 caracteres")]
        [Display(Name = "Entidad bancaria")]
        public string EntidadBancaria { get; set; } = string.Empty;
        
        [StringLength(50, ErrorMessage = "La moneda no puede exceder 50 caracteres")]
        [Display(Name = "Moneda")]
        public string? Moneda { get; set; }
        
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
        
        [Required(ErrorMessage = "El saldo inicial es obligatorio")]
        [Display(Name = "Saldo inicial")]
        public decimal SaldoInicial { get; set; } = 0;
        
        [Display(Name = "Saldo actual")]
        public decimal SaldoActual { get; set; } = 0;
        
        [Display(Name = "Saldo conciliado")]
        public decimal SaldoConciliado { get; set; } = 0;
        
        [Display(Name = "Pendiente de conciliar")]
        public decimal SaldoPendienteReconciliacion => SaldoActual - SaldoConciliado;
        
        [Required(ErrorMessage = "La fecha de apertura es obligatoria")]
        [Display(Name = "Fecha de apertura")]
        [DataType(DataType.Date)]
        public DateTime FechaApertura { get; set; } = DateTime.Now;
        
        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
        
        [Display(Name = "Cuenta contable asociada")]
        public int CuentaContableId { get; set; }
        
        [Display(Name = "Nombre de cuenta contable")]
        public string? CuentaContableNombre { get; set; }
        
        [Display(Name = "Logo del banco")]
        public string? LogoUrl { get; set; }
        
        [Display(Name = "Cargar logo del banco")]
        public IFormFile? LogoFile { get; set; }
    }
    
    public class TransaccionBancariaViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "La cuenta bancaria es obligatoria")]
        [Display(Name = "Cuenta bancaria")]
        public int BancoId { get; set; }
        
        [Display(Name = "Nombre de cuenta")]
        public string? BancoNombre { get; set; }
        
        [Required(ErrorMessage = "El tipo de transacción es obligatorio")]
        [Display(Name = "Tipo de transacción")]
        public TipoTransaccionBancaria Tipo { get; set; }
        
        [Required(ErrorMessage = "El monto es obligatorio")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }
        
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now;
        
        [Required(ErrorMessage = "El concepto es obligatorio")]
        [StringLength(200, ErrorMessage = "El concepto no puede exceder 200 caracteres")]
        [Display(Name = "Concepto")]
        public string Concepto { get; set; } = string.Empty;
        
        [StringLength(100, ErrorMessage = "La referencia no puede exceder 100 caracteres")]
        [Display(Name = "Referencia")]
        public string? Referencia { get; set; }
        
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
        
        [Display(Name = "Contacto")]
        public int? ContactoId { get; set; }
        
        [Display(Name = "Nombre del contacto")]
        public string? ContactoNombre { get; set; }
        
        [Display(Name = "Cuenta destino")]
        public int? BancoDestinoId { get; set; }
        
        [Display(Name = "Nombre de cuenta destino")]
        public string? BancoDestinoNombre { get; set; }
        
        [Display(Name = "Cuenta destino ID con prefijo")]
        public string? BancoDestinoIdString { get; set; }
        
        // Propiedad para transferencias a cuentas contables (no bancos)
        [Display(Name = "Cuenta contable destino")]
        public int? CuentaContableDestinoId { get; set; }
    }
} 