using System.Collections.Generic;

namespace SistemaContable.Utilities
{
    public static class Constantes
    {
        public static class TiposComprobanteFiscal
        {
            public const string CREDITO_FISCAL = "B01 - Crédito fiscal";
            public const string CONSUMO = "B02 - Consumo";
            public const string REGIMEN_ESPECIAL = "B14 - Régimen especial de tributación";
            public const string GUBERNAMENTAL = "B15 - Gubernamentales";
            public const string EXPORTACION = "B16 - Comprobante para exportación";
            public const string SIN_NCF = "Sin NCF";
            
            public static List<string> ObtenerTipos()
            {
                return new List<string>
                {
                    CREDITO_FISCAL,
                    CONSUMO,
                    REGIMEN_ESPECIAL,
                    GUBERNAMENTAL,
                    EXPORTACION,
                    SIN_NCF
                };
            }
        }

        public static class TiposDocumento
        {
            public const string FACTURA_VENTA = "Factura de venta";
            public const string NOTA_CREDITO = "Nota de crédito";
            public const string NOTA_DEBITO = "Nota débito";
            public const string RECIBO_CAJA = "Recibo de caja";
            public const string GASTO = "Gasto";
            public const string FACTURA_PROVEEDOR = "Factura de proveedor";
            public const string COTIZACION = "Cotización";
            public const string ORDEN_COMPRA = "Orden de compra";
            public const string CONDUCE = "Conduce";
            public const string AJUSTE_INVENTARIO = "Ajuste de inventario";
            
            public static List<string> ObtenerTiposDocumento()
            {
                return new List<string>
                {
                    FACTURA_VENTA,
                    NOTA_CREDITO,
                    NOTA_DEBITO,
                    RECIBO_CAJA,
                    GASTO,
                    FACTURA_PROVEEDOR,
                    COTIZACION,
                    ORDEN_COMPRA,
                    CONDUCE,
                    AJUSTE_INVENTARIO
                };
            }
        }
    }
} 