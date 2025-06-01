using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class Usuario : BaseEntity
    {
        [Required(ErrorMessage = "El nombre completo es requerido")]
        [StringLength(100, ErrorMessage = "El nombre completo no puede exceder 100 caracteres")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres")]
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [StringLength(256)] // SHA256 produce 64 caracteres hexadecimales
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(4, MinimumLength = 4, ErrorMessage = "El PIN POS debe tener exactamente 4 dígitos")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "El PIN POS debe contener solo números")]
        [Display(Name = "PIN POS")]
        public string? PinPOS { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [Display(Name = "Correo Electrónico")]
        public string? Email { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [StringLength(100, ErrorMessage = "La ciudad no puede exceder 100 caracteres")]
        [Display(Name = "Ciudad")]
        public string? Ciudad { get; set; }

        [StringLength(100, ErrorMessage = "El estado/provincia no puede exceder 100 caracteres")]
        [Display(Name = "Estado/Provincia")]
        public string? EstadoProvincia { get; set; }

        [StringLength(20, ErrorMessage = "El código postal no puede exceder 20 caracteres")]
        [Display(Name = "Código Postal")]
        public string? CodigoPostal { get; set; }

        [Display(Name = "Foto de Perfil")]
        public string? FotoUrl { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        [Display(Name = "Rol")]
        public int RolId { get; set; }

        // Propiedades adicionales requeridas
        [Required]
        public int EmpresaId { get; set; }
        
        public bool Activo { get; set; } = true;

        // Navegación
        public virtual Rol? Rol { get; set; }
        public virtual Empresa? Empresa { get; set; }
    }
}