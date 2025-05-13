using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class ItemAlmacen : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        [Required]
        public int AlmacenId { get; set; }
        [ForeignKey("AlmacenId")]
        public virtual Almacen? Almacen { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Stock { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal NivelMinimo { get; set; }

        [StringLength(100)]
        public string? Ubicacion { get; set; }

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}