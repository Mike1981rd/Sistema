using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string NumeroIdentificacion { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string TipoIdentificacion { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Direccion { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Ciudad { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Provincia { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string CodigoPostal { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Pais { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string SitioWeb { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string NombreComercial { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string MonedaPrincipal { get; set; } = string.Empty;
        
        public int NumeroEmpleados { get; set; }
        
        public int PrecisionDecimal { get; set; }
        
        [Required]
        [StringLength(5)]
        public string SeparadorDecimal { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string LogoUrl { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string ResponsabilidadTributaria { get; set; } = string.Empty;
        
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        public DateTime? FechaActualizacion { get; set; }
        
        public bool Activo { get; set; }
        
        // Navigation Properties
        public virtual ICollection<CuentaContable>? CuentasContables { get; set; }
    }
} 