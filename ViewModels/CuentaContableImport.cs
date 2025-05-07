using System.Collections.Generic;

namespace SistemaContable.ViewModels
{
    public class CuentaContableImport
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? CuentaPadre { get; set; }
        public string? CodigoPadre { get; set; }
        public string? Naturaleza { get; set; }
        public string? TipoCuenta { get; set; }
        public string? Categoria { get; set; }
        public string? UsoCuenta { get; set; }
        public bool VerSaldoPorTercero { get; set; }
        public int Nivel { get; set; } = 1;
        public List<string> Errores { get; set; } = new List<string>();
        public List<string> Advertencias { get; set; } = new List<string>();
    }
} 