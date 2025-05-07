using SistemaContable.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels
{
    public class SaldosInicialesViewModel
    {
        [Required(ErrorMessage = "La fecha inicial es requerida")]
        [Display(Name = "Fecha inicial")]
        public DateTime FechaInicial { get; set; } = DateTime.Today;
        
        public List<CuentaContable> CuentasActivo { get; set; } = new List<CuentaContable>();
        public List<CuentaContable> CuentasPasivo { get; set; } = new List<CuentaContable>();
        public List<CuentaContable> CuentasPatrimonio { get; set; } = new List<CuentaContable>();
        
        public Dictionary<int, SaldoInicial> SaldosIniciales { get; set; } = new Dictionary<int, SaldoInicial>();
        
        public List<SaldoInicialInput> SaldosIngresados { get; set; } = new List<SaldoInicialInput>();
        
        public class SaldoInicialInput
        {
            public int CuentaContableId { get; set; }
            
            public int? ContactoId { get; set; }
            
            public decimal Valor { get; set; }
        }
    }
} 