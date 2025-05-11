using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class ComprobanteFiscal
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del comprobante es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Tipo de documento")]
        public string TipoDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de comprobante es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = string.Empty;

        [Display(Name = "Preferida")]
        public bool Preferida { get; set; } = false;

        [Display(Name = "Electrónica")]
        public bool Electronica { get; set; } = false;

        [StringLength(10)]
        [Required(ErrorMessage = "El prefijo es obligatorio")]
        [Display(Name = "Prefijo")]
        public string Prefijo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número inicial es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El número inicial debe ser mayor a 0")]
        [Display(Name = "Número inicial")]
        public int NumeroInicial { get; set; } = 1;

        [Display(Name = "Número final")]
        [Range(0, int.MaxValue, ErrorMessage = "El número final debe ser mayor o igual a 0")]
        [CustomValidation(typeof(ComprobanteFiscal), "ValidarNumeroFinal")]
        public int? NumeroFinal { get; set; }

        [Display(Name = "Siguiente número")]
        public int SiguienteNumero { get; set; } = 1;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de finalización")]
        public DateTime? FechaFinalizacion { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        [Display(Name = "Fecha de vencimiento")]
        [CustomValidation(typeof(ComprobanteFiscal), "ValidarFechaVencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "La sucursal es obligatoria")]
        [Display(Name = "Sucursal")]
        public string Sucursal { get; set; } = "Principal";

        [Display(Name = "Fecha de creación")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Display(Name = "Última modificación")]
        [DataType(DataType.DateTime)]
        public DateTime UltimaModificacion { get; set; } = DateTime.Now;

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
        
        // Métodos de validación estáticos
        public static ValidationResult ValidarFechaVencimiento(DateTime fecha, ValidationContext context)
        {
            if (fecha.Date < DateTime.Now.Date)
            {
                return new ValidationResult("La fecha de vencimiento debe ser futura");
            }
            
            return ValidationResult.Success!;
        }

        public static ValidationResult ValidarNumeroFinal(int? numeroFinal, ValidationContext context)
        {
            if (numeroFinal.HasValue)
            {
                var instance = (ComprobanteFiscal)context.ObjectInstance;
                if (numeroFinal.Value <= instance.NumeroInicial)
                {
                    return new ValidationResult("El número final debe ser mayor que el número inicial");
                }
                
                if (numeroFinal.Value < instance.SiguienteNumero)
                {
                    return new ValidationResult("El número final debe ser mayor o igual al siguiente número");
                }
            }
            
            return ValidationResult.Success!;
        }
    }
} 