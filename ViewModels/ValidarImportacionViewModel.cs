using System.Collections.Generic;

namespace SistemaContable.ViewModels
{
    public class ValidarImportacionViewModel
    {
        public List<CuentaContableImport> CuentasImportadas { get; set; } = new List<CuentaContableImport>();
        public List<string> Errores { get; set; } = new List<string>();
        public List<string> Advertencias { get; set; } = new List<string>();
    }
} 