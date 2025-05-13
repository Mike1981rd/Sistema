using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class ItemTara : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        [Required]
        public int ItemContenedorId { get; set; }
        [ForeignKey("ItemContenedorId")]
        public virtual ItemContenedor? ItemContenedor { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ValorTara { get; set; }

        [StringLength(100)]
        public string? Observacion { get; set; }

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }

        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
}