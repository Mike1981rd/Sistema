using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels.Productos
{
    /// <summary>
    /// DTO para manejar un nivel de precio específico de un producto
    /// </summary>
    public class ProductoPrecioDto
    {
        /// <summary>
        /// ID del precio (para edición, null para creación)
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Nombre descriptivo del nivel de precio
        /// </summary>
        [Required(ErrorMessage = "El nombre del nivel de precio es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombreNivel { get; set; } = "Precio Base";

        /// <summary>
        /// Precio base sin impuestos
        /// </summary>
        [Required(ErrorMessage = "El precio base es requerido")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio base debe ser mayor a 0")]
        public decimal PrecioBase { get; set; }

        /// <summary>
        /// Precio total con impuestos incluidos
        /// </summary>
        [Required(ErrorMessage = "El precio total es requerido")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio total debe ser mayor a 0")]
        public decimal PrecioTotal { get; set; }

        /// <summary>
        /// Lista de IDs de impuestos que aplican a este nivel de precio
        /// Si está vacía, hereda los impuestos del producto
        /// </summary>
        public List<int> ImpuestoIds { get; set; } = new List<int>();

        /// <summary>
        /// Orden de visualización
        /// </summary>
        public int Orden { get; set; } = 0;

        /// <summary>
        /// Indica si este es el precio principal del producto
        /// </summary>
        public bool EsPrincipal { get; set; } = false;

        /// <summary>
        /// Referencia opcional a lista de precios
        /// </summary>
        public int? ListaPrecioId { get; set; }

        /// <summary>
        /// Descripción opcional del nivel de precio
        /// </summary>
        [StringLength(255, ErrorMessage = "La descripción no puede exceder 255 caracteres")]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Estado activo/inactivo
        /// </summary>
        public bool Activo { get; set; } = true;
    }

    /// <summary>
    /// DTO para respuesta de precio con información de impuestos expandida
    /// </summary>
    public class ProductoPrecioDetalleDto : ProductoPrecioDto
    {
        /// <summary>
        /// Información detallada de los impuestos aplicados
        /// </summary>
        public List<ImpuestoAplicadoDto> ImpuestosAplicados { get; set; } = new List<ImpuestoAplicadoDto>();

        /// <summary>
        /// Porcentaje total de impuestos aplicados
        /// </summary>
        public decimal PorcentajeTotalImpuestos { get; set; }
    }

    /// <summary>
    /// DTO para información de impuesto aplicado a un precio
    /// </summary>
    public class ImpuestoAplicadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Porcentaje { get; set; }
        public decimal? PorcentajeOverride { get; set; }
        public decimal PorcentajeEfectivo { get; set; }
        public int Orden { get; set; }
    }
}