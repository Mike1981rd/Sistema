using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models.ViewModels
{
    public class MarcaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
} 