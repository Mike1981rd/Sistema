using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class Contenedor
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
    }
} 