using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class Contacto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Identificacion { get; set; }

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        public string? Direccion { get; set; }

        public bool EsCliente { get; set; }

        public bool EsProveedor { get; set; }
        
        public bool Activo { get; set; } = true;

        [Required]
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
} 