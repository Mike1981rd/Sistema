using System;

namespace SistemaContable.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
} 