using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    [Obsolete("Esta clase está obsoleta y será eliminada en una futura versión.")]
    public class FamiliaCuentaContable
    {
        public int FamiliaId { get; set; }
        public int CuentaContableId { get; set; }
        
        public virtual Familia Familia { get; set; }
        public virtual CuentaContable CuentaContable { get; set; }
    }
} 