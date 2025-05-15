using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels
{
    public class ProductoModificadorGrupoViewModel
    {
        public int ProductoId { get; set; }
        public int GrupoModificadoresId { get; set; }

        [Display(Name = "Orden Específico")]
        public int OrdenEspecificoProducto { get; set; } = 0;

        // Para mostrar información del grupo
        public string? NombreGrupo { get; set; }
        public bool EsForzado { get; set; }
        public int MinSeleccion { get; set; }
        public int MaxSeleccion { get; set; }

        // Para selección en la UI
        public bool Seleccionado { get; set; }
    }
}