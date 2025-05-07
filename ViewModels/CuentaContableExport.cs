namespace SistemaContable.ViewModels
{
    public class CuentaContableExport
    {
        public int Nivel { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Naturaleza { get; set; } = string.Empty;
        public string? UsoCuenta { get; set; }
        public string? TipoCuenta { get; set; } 
        public string? Descripcion { get; set; }
    }
} 