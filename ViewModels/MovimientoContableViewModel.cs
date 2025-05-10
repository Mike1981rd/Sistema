using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.ViewModels
{
    public class MovimientoContableViewModel
    {
        public int Id { get; set; }
        
        public int EntradaDiarioId { get; set; }
        
        [Required(ErrorMessage = "La cuenta contable es obligatoria")]
        [DisplayName("Cuenta contable")]
        public int CuentaContableId { get; set; }
        
        [DisplayName("Contacto")]
        public int? ContactoId { get; set; }
        
        [DisplayName("Tipo de contacto")]
        public string TipoContacto { get; set; }
        
        [DisplayName("Nº de documento")]
        [StringLength(30)]
        public string NumeroDocumento { get; set; }
        
        [DisplayName("Descripción")]
        [StringLength(200)]
        public string Descripcion { get; set; }
        
        [DisplayName("Débito")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Range(0, 999999999999.99, ErrorMessage = "El valor debe ser mayor o igual a 0")]
        public decimal Debito { get; set; }
        
        // Propiedad para recibir el texto formateado con separador de miles
        public string DebitoStr { get; set; }
        
        [DisplayName("Crédito")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Range(0, 999999999999.99, ErrorMessage = "El valor debe ser mayor o igual a 0")]
        public decimal Credito { get; set; }
        
        // Propiedad para recibir el texto formateado con separador de miles
        public string CreditoStr { get; set; }
        
        public string CuentaContableNombre { get; set; }
        public string ContactoNombre { get; set; }
    }
} 