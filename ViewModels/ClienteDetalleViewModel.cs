using SistemaContable.Models;

namespace SistemaContable.ViewModels
{
    public class ClienteDetalleViewModel
    {
        public required Cliente Cliente { get; set; }
        
        // Información financiera
        public decimal CuentasPorCobrar { get; set; }
        public decimal AnticiposRecibidos { get; set; }
        public decimal AnticiposEntregados { get; set; }
        public decimal PorPagar { get; set; }
        public decimal NotasCredito { get; set; }
        public decimal NotasDebito { get; set; }
    }
} 