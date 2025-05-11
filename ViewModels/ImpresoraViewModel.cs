using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaContable.ViewModels
{
    public class ImpresoraViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [MaxLength(100, ErrorMessage = "El modelo no puede exceder los 100 caracteres")]
        [DisplayName("Modelo")]
        public string Modelo { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "La ruta física no puede exceder los 100 caracteres")]
        [DisplayName("Ruta Física")]
        public string? RutasFisicas { get; set; }
        
        // Lista de rutas físicas disponibles
        public SelectList? RutasFisicasDisponibles { get; set; }

        [Required]
        [DisplayName("Estado")]
        public bool Estado { get; set; } = true;

        // Campos requeridos por la base de datos
        [Required]
        public int EmpresaId { get; set; }
    }
} 