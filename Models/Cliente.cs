using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre o razón social es obligatorio")]
        [Display(Name = "Nombre o Razón Social")]
        [StringLength(150)]
        public string NombreRazonSocial { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de identificación es obligatorio")]
        [Display(Name = "Tipo de Identificación")]
        public int TipoIdentificacionId { get; set; }

        [ForeignKey("TipoIdentificacionId")]
        public TipoIdentificacion? TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "El número de identificación es obligatorio")]
        [Display(Name = "Número de Identificación")]
        [StringLength(20)]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [Display(Name = "Municipio")]
        public int? MunicipioId { get; set; }

        [ForeignKey("MunicipioId")]
        public Municipio? Municipio { get; set; }

        [Display(Name = "País")]
        public int? PaisId { get; set; }

        [ForeignKey("PaisId")]
        public Pais? Pais { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(200)]
        public string? Direccion { get; set; }

        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Display(Name = "Teléfono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Display(Name = "Celular")]
        [StringLength(20)]
        public string? Celular { get; set; }

        [Display(Name = "Comprobante Fiscal")]
        public int? TipoNcfId { get; set; }

        [ForeignKey("TipoNcfId")]
        public ComprobanteFiscal? TipoNcf { get; set; }

        [Display(Name = "Plazo de Pago")]
        public int? PlazoPagoId { get; set; }

        [ForeignKey("PlazoPagoId")]
        public PlazoPago? PlazoPago { get; set; }

        [Display(Name = "Lista de Precios")]
        public int? ListaPrecioId { get; set; }

        [ForeignKey("ListaPrecioId")]
        public ListaPrecio? ListaPrecio { get; set; }

        [Display(Name = "Vendedor")]
        public int? VendedorId { get; set; }

        [ForeignKey("VendedorId")]
        public Vendedor? Vendedor { get; set; }

        [Display(Name = "Límite de Crédito")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LimiteCredito { get; set; }

        [Display(Name = "Es Cliente")]
        public bool EsCliente { get; set; } = true;

        [Display(Name = "Es Proveedor")]
        public bool EsProveedor { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(250)]
        public string? ImagenUrl { get; set; }

        [Display(Name = "Cuenta por Cobrar")]
        public int? CuentaPorCobrarId { get; set; }

        [ForeignKey("CuentaPorCobrarId")]
        public CuentaContable? CuentaPorCobrar { get; set; }

        [Display(Name = "Cuenta por Pagar")]
        public int? CuentaPorPagarId { get; set; }

        [ForeignKey("CuentaPorPagarId")]
        public CuentaContable? CuentaPorPagar { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }

        // Propiedad calculada para mostrar el tipo (Cliente, Proveedor o ambos)
        [NotMapped]
        public string Tipo
        {
            get
            {
                if (EsCliente && EsProveedor)
                    return "Cliente/Proveedor";
                if (EsCliente)
                    return "Cliente";
                if (EsProveedor)
                    return "Proveedor";
                return "No definido";
            }
        }
    }
} 