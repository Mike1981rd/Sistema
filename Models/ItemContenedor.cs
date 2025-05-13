using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class ItemContenedor : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        [Required]
        public int UnidadMedidaId { get; set; }
        [ForeignKey("UnidadMedidaId")]
        public virtual UnidadMedida? UnidadMedida { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(150)]
        public string Etiqueta { get; set; }

        // Relación con el contenedor superior (para jerarquía)
        public int? ContenedorSuperiorId { get; set; }
        [ForeignKey("ContenedorSuperiorId")]
        public virtual ItemContenedor? ContenedorSuperior { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Factor { get; set; } = 1;

        [Column(TypeName = "decimal(18,4)")]
        public decimal Costo { get; set; }

        // Indica si es el contenedor principal (base)
        public bool EsPrincipal { get; set; }

        // Indica si este contenedor se usa para compras
        public bool EsContenedorCompra { get; set; }

        // Orden en la jerarquía
        public int Orden { get; set; }

        // Auditoría
        public int? UsuarioCreacionId { get; set; }
        public int? UsuarioModificacionId { get; set; }
        
        // Campo para el ID de empresa (multiempresa)
        public int EmpresaId { get; set; }
    }
} 