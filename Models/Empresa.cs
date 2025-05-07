using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre de la empresa es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = "Empresa por defecto";
        
        [Required(ErrorMessage = "El número de identificación es requerido")]
        [StringLength(20, ErrorMessage = "El número de identificación no puede exceder los 20 caracteres")]
        public string NumeroIdentificacion { get; set; } = "000000000";
        
        [Required(ErrorMessage = "El tipo de identificación es requerido")]
        [StringLength(20)]
        public string TipoIdentificacion { get; set; } = "RNC";
        
        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(200)]
        public string Direccion { get; set; } = "Dirección por defecto";
        
        [Required(ErrorMessage = "La ciudad es requerida")]
        [StringLength(100)]
        public string Ciudad { get; set; } = "Ciudad por defecto";
        
        [Required(ErrorMessage = "La provincia es requerida")]
        [StringLength(100)]
        public string Provincia { get; set; } = "Provincia por defecto";
        
        [StringLength(20)]
        public string CodigoPostal { get; set; } = "";
        
        [Required(ErrorMessage = "El país es requerido")]
        [StringLength(50)]
        public string Pais { get; set; } = "República Dominicana";
        
        [StringLength(20)]
        public string Telefono { get; set; } = "";
        
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Email no válido")]
        public string Email { get; set; } = "";
        
        [StringLength(100)]
        public string SitioWeb { get; set; } = "";
        
        [Required(ErrorMessage = "La moneda principal es requerida")]
        [StringLength(20)]
        public string MonedaPrincipal { get; set; } = "DOP";
        
        public int NumeroEmpleados { get; set; } = 0;
        
        public int PrecisionDecimal { get; set; } = 2;
        
        [Required(ErrorMessage = "El separador decimal es requerido")]
        [StringLength(5)]
        public string SeparadorDecimal { get; set; } = ".";
        
        [StringLength(255)]
        public string LogoUrl { get; set; } = "";
        
        [StringLength(50)]
        public string ResponsabilidadTributaria { get; set; } = "Régimen General";
        
        [StringLength(100)]
        public string NombreComercial { get; set; } = "";
        
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        public DateTime? FechaActualizacion { get; set; }
        
        public bool Activo { get; set; } = true;
    }
} 