using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SistemaContable.ViewModels
{
    public class EntradaDiarioViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DisplayName("Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        
        [Required(ErrorMessage = "El tipo de entrada es obligatorio")]
        [DisplayName("Tipo de entrada de diario")]
        public int TipoEntradaId { get; set; }
        
        [Required(ErrorMessage = "La numeración es obligatoria")]
        [DisplayName("Numeración")]
        public int NumeracionId { get; set; }
        
        [DisplayName("Código")]
        public string Codigo { get; set; }
        
        [DisplayName("Observaciones")]
        [StringLength(500)]
        public string Observaciones { get; set; }
        
        [DisplayName("Estado")]
        public string Estado { get; set; }
        
        public List<MovimientoContableViewModel> Movimientos { get; set; } = new List<MovimientoContableViewModel>();
        
        public SelectList TiposEntrada { get; set; }
        public SelectList Numeraciones { get; set; }
        
        [DisplayName("Total Débito")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalDebito => Movimientos.Sum(m => m.Debito);
        
        [DisplayName("Total Crédito")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalCredito => Movimientos.Sum(m => m.Credito);
        
        [DisplayName("Diferencia")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Diferencia => TotalDebito - TotalCredito;
    }
} 