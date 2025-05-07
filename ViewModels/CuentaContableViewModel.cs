using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels
{
    public class CuentaContableViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;
        
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public string Categoria { get; set; } = "Activo";
        
        [Required(ErrorMessage = "El tipo de cuenta es obligatorio")]
        public string TipoCuenta { get; set; } = "Movimiento";
        
        public string? UsoCuenta { get; set; }
        
        [Required(ErrorMessage = "La naturaleza es obligatoria")]
        public string Naturaleza { get; set; } = "Deudora";
        
        public string? Descripcion { get; set; }
        
        public bool VerSaldoPorTercero { get; set; } = false;
        
        public int? CuentaPadreId { get; set; }
        
        public string? CuentaPadreNombre { get; set; }
        
        [Required]
        public int Nivel { get; set; } = 1;
        
        public bool EsCuentaSistema { get; set; } = false;
    }
} 