using SistemaContable.Models;
using System;
using System.Collections.Generic;

namespace SistemaContable.ViewModels
{
    public class MovimientosCuentaViewModel
    {
        public CuentaContable? Cuenta { get; set; }
        
        public DateTime FechaInicio { get; set; } = new DateTime(DateTime.Now.Year, 1, 1);
        
        public DateTime FechaFin { get; set; } = DateTime.Now;
        
        public List<MovimientoCuenta> Movimientos { get; set; } = new List<MovimientoCuenta>();
        
        public decimal SaldoInicial { get; set; } = 0;
        
        public decimal SaldoFinal { get; set; } = 0;
        
        public decimal TotalDebitos { get; set; } = 0;
        
        public decimal TotalCreditos { get; set; } = 0;
        
        public class MovimientoCuenta
        {
            public DateTime Fecha { get; set; }
            
            public string Documento { get; set; } = string.Empty;
            
            public int NumeroDocumento { get; set; }
            
            public string Detalle { get; set; } = string.Empty;
            
            public string Tercero { get; set; } = string.Empty;
            
            public decimal Debito { get; set; } = 0;
            
            public decimal Credito { get; set; } = 0;
            
            public decimal SaldoMovimiento { get; set; } = 0;
            
            public decimal SaldoAcumulado { get; set; } = 0;
        }
    }
} 