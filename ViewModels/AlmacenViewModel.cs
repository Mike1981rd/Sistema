using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels
{
    public class AlmacenViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del almacén es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [DisplayName("Nombre del almacén")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [MaxLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        [DisplayName("Correo electrónico")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [MaxLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [DisplayName("Estado")]
        public bool Estado { get; set; } = true;

        // Campos requeridos por la base de datos
        [Required]
        public int EmpresaId { get; set; }
    }
} 