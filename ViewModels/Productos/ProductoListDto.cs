using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels.Productos
{
    /// <summary>
    /// DTO para mostrar productos en listados
    /// </summary>
    public class ProductoListDto
    {
        public int Id { get; set; }
        
        public string Nombre { get; set; }
        
        public string NombreCortoTPV { get; set; }
        
        public decimal PrecioVenta { get; set; }
        
        public string CategoriaNombre { get; set; }
        
        public int CategoriaId { get; set; }
        
        public bool EsActivo { get; set; }
        
        public string? ImagenUrl { get; set; }
        
        public string? ColorBotonTPV { get; set; }
        
        public string? PLU { get; set; }
        
        public int NumeroVariantes { get; set; }
        
        public bool PermiteModificadores { get; set; }
    }
}