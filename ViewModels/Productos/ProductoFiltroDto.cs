namespace SistemaContable.ViewModels.Productos
{
    /// <summary>
    /// DTO para filtrado y paginación de productos
    /// </summary>
    public class ProductoFiltroDto
    {
        // Paginación
        public int Pagina { get; set; } = 1;
        public int TamanoPagina { get; set; } = 20;
        
        // Filtros
        public int? CategoriaId { get; set; }
        public int? ImpuestoId { get; set; }
        public bool? EsActivo { get; set; }
        public string? TextoBusqueda { get; set; }
        public decimal? PrecioMinimo { get; set; }
        public decimal? PrecioMaximo { get; set; }
        public bool? PermiteModificadores { get; set; }
        public bool? DisponibleParaVenta { get; set; }
        
        // Ordenamiento
        public string? OrdenarPor { get; set; } // nombre, precio, categoria, fecha
        public bool OrdenDescendente { get; set; } = false;
    }
}