using System.Collections.Generic;

namespace SistemaContable.Models
{
    public static class DataLists
    {
        public static List<Country> LatinAmericanCountries = new List<Country>
        {
            new Country { Code = "DO", Name = "República Dominicana", Currency = "DOP", CurrencyName = "Peso Dominicano" },
            new Country { Code = "AR", Name = "Argentina", Currency = "ARS", CurrencyName = "Peso Argentino" },
            new Country { Code = "BO", Name = "Bolivia", Currency = "BOB", CurrencyName = "Boliviano" },
            new Country { Code = "BR", Name = "Brasil", Currency = "BRL", CurrencyName = "Real Brasileño" },
            new Country { Code = "CL", Name = "Chile", Currency = "CLP", CurrencyName = "Peso Chileno" },
            new Country { Code = "CO", Name = "Colombia", Currency = "COP", CurrencyName = "Peso Colombiano" },
            new Country { Code = "CR", Name = "Costa Rica", Currency = "CRC", CurrencyName = "Colón Costarricense" },
            new Country { Code = "CU", Name = "Cuba", Currency = "CUP", CurrencyName = "Peso Cubano" },
            new Country { Code = "EC", Name = "Ecuador", Currency = "USD", CurrencyName = "Dólar Estadounidense" },
            new Country { Code = "SV", Name = "El Salvador", Currency = "USD", CurrencyName = "Dólar Estadounidense" },
            new Country { Code = "GT", Name = "Guatemala", Currency = "GTQ", CurrencyName = "Quetzal" },
            new Country { Code = "HN", Name = "Honduras", Currency = "HNL", CurrencyName = "Lempira" },
            new Country { Code = "MX", Name = "México", Currency = "MXN", CurrencyName = "Peso Mexicano" },
            new Country { Code = "NI", Name = "Nicaragua", Currency = "NIO", CurrencyName = "Córdoba" },
            new Country { Code = "PA", Name = "Panamá", Currency = "PAB", CurrencyName = "Balboa" },
            new Country { Code = "PY", Name = "Paraguay", Currency = "PYG", CurrencyName = "Guaraní" },
            new Country { Code = "PE", Name = "Perú", Currency = "PEN", CurrencyName = "Sol" },
            new Country { Code = "PR", Name = "Puerto Rico", Currency = "USD", CurrencyName = "Dólar Estadounidense" },
            new Country { Code = "UY", Name = "Uruguay", Currency = "UYU", CurrencyName = "Peso Uruguayo" },
            new Country { Code = "VE", Name = "Venezuela", Currency = "VES", CurrencyName = "Bolívar Soberano" }
        };
    }

    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }
    }
} 