using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario o correo es requerido")]
        [Display(Name = "Usuario o Correo")]
        public string LoginIdentifier { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
        public string? ReturnUrl { get; set; }
    }
}