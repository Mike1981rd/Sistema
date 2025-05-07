using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre de la empresa es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El número de identificación es requerido")]
        [StringLength(20, ErrorMessage = "El número de identificación no puede exceder los 20 caracteres")]
        public string NumeroIdentificacion { get; set; }
        
        [StringLength(20)]
        public string TipoIdentificacion { get; set; } // RNC, NIT, RUC, etc.
        
        [StringLength(200)]
        public string Direccion { get; set; }
        
        [StringLength(100)]
        public string Ciudad { get; set; }
        
        [StringLength(100)]
        public string Provincia { get; set; }
        
        [StringLength(20)]
        public string CodigoPostal { get; set; }
        
        [StringLength(50)]
        public string Pais { get; set; }
        
        [StringLength(20)]
        public string Telefono { get; set; }
        
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Email no válido")]
        public string Email { get; set; }
        
        [StringLength(100)]
        public string SitioWeb { get; set; }
        
        [StringLength(20)]
        public string MonedaPrincipal { get; set; }
        
        public int NumeroEmpleados { get; set; }
        
        public int PrecisionDecimal { get; set; } = 2;
        
        [StringLength(5)]
        public string SeparadorDecimal { get; set; } = ",";
        
        [StringLength(255)]
        public string LogoUrl { get; set; }
        
        [StringLength(50)]
        public string ResponsabilidadTributaria { get; set; }
        
        [StringLength(100)]
        public string NombreComercial { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        
        public DateTime? FechaActualizacion { get; set; }
        
        public bool Activo { get; set; } = true;
    }
} 