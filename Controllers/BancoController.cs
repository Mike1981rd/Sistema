using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.ViewModels;
using SistemaContable.Services;

namespace SistemaContable.Controllers
{
    public class BancoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public BancoController(ApplicationDbContext context, IEmpresaService empresaService)
        {
            _context = context;
            _empresaService = empresaService;
        }

        // GET: Banco
        public async Task<IActionResult> Index()
        {
            var bancos = await _context.Bancos
                .Include(b => b.CuentaContable)
                .OrderBy(b => b.Nombre)
                .ToListAsync();
            
            return View(bancos);
        }

        // GET: Banco/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banco = await _context.Bancos
                .Include(b => b.CuentaContable)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (banco == null)
            {
                return NotFound();
            }

            // Obtener las últimas transacciones
            var transacciones = await _context.TransaccionesBanco
                .Where(t => t.BancoId == id)
                .OrderByDescending(t => t.Fecha)
                .Take(10)
                .ToListAsync();
                
            ViewBag.Transacciones = transacciones;

            return View(banco);
        }

        // GET: Banco/Create
        public IActionResult Create()
        {
            // Ya no buscamos o creamos la cuenta contable automáticamente
            // El campo se ocultará en la vista
            
            // Obtener lista de monedas de los países para el dropdown
            var monedas = DataLists.LatinAmericanCountries
                .Select(c => new { Value = c.Currency, Text = $"{c.Currency} - {c.CurrencyName}" })
                .Distinct()
                .OrderBy(c => c.Text)
                .ToList();
                
            ViewBag.Monedas = new SelectList(monedas, "Value", "Text");
            
            // Inicializar el viewmodel sin preestablacer la cuenta contable
            return View(new BancoViewModel());
        }

        // POST: Banco/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BancoViewModel viewModel)
        {
            // Obtener el ID de empresa usando el servicio
            var EmpresaId = await _empresaService.ObtenerEmpresaActualId();
            
            // Preparar monedas para la vista (se usará en caso de error)
            var monedasList = DataLists.LatinAmericanCountries
                .Select(c => new { Value = c.Currency, Text = $"{c.Currency} - {c.CurrencyName}" })
                .Distinct()
                .OrderBy(c => c.Text)
                .ToList();
                
            ViewBag.Monedas = new SelectList(monedasList, "Value", "Text", viewModel.Moneda);
            
            try
            {
                if (ModelState.IsValid)
                {
                    // Verificar que la empresa existe
                    var empresaExiste = await _context.Empresas.AnyAsync(e => e.Id == EmpresaId);
                    if (!empresaExiste)
                    {
                        // Si la empresa no existe, la creamos automáticamente
                        var empresa = new Empresa
                        {
                            Id = EmpresaId,
                            Nombre = "Empresa Default",
                            NumeroIdentificacion = "00000000001",
                            TipoIdentificacion = "RNC",
                            Direccion = "Dirección por defecto",
                            Ciudad = "Ciudad",
                            Provincia = "Provincia",
                            CodigoPostal = "00000",
                            Pais = "República Dominicana",
                            Telefono = "000-000-0000",
                            Email = "contacto@ejemplo.com",
                            SitioWeb = "www.ejemplo.com",
                            NombreComercial = "Empresa Default",
                            MonedaPrincipal = "DOP",
                            PrecisionDecimal = 2,
                            SeparadorDecimal = ".",
                            LogoUrl = "/img/default-logo.png",
                            ResponsabilidadTributaria = "Normal",
                            Activo = true,
                            FechaCreacion = DateTime.UtcNow
                        };

                        _context.Empresas.Add(empresa);
                        await _context.SaveChangesAsync();
                    }
                    
                    // Usar una transacción para garantizar la integridad de los datos
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    
                    try
                    {
                        // 1. Buscar o crear la estructura de categorías contables para bancos
                        CuentaContable? categoriaActivo;
                        CuentaContable? categoriaActivosCorrientes;
                        CuentaContable? categoriaBancos;
                        CuentaContable cuentaContableBanco;
                        
                        // Buscar/Crear categoría Activo
                        categoriaActivo = await _context.CuentasContables
                            .FirstOrDefaultAsync(c => c.Categoria == "Activo" && c.Nombre == "Activo" && c.CuentaPadreId == null);
                            
                        if (categoriaActivo == null)
                        {
                            categoriaActivo = new CuentaContable
                            {
                                Codigo = "1",
                                Nombre = "Activo",
                                Descripcion = "Activos de la empresa",
                                Categoria = "Activo",
                                Naturaleza = "Deudora",
                                TipoCuenta = "Mayor",
                                Nivel = 1,
                                Orden = 1,
                                Activo = true,
                                EmpresaId = EmpresaId,
                                FechaCreacion = DateTime.UtcNow
                            };
                            _context.CuentasContables.Add(categoriaActivo);
                            await _context.SaveChangesAsync();
                            
                            // Verificar que se guardó correctamente
                            if (categoriaActivo.Id <= 0)
                            {
                                throw new Exception("No se pudo crear la categoría contable 'Activo'.");
                            }
                        }
                        
                        // Buscar/Crear categoría Activos Corrientes
                        categoriaActivosCorrientes = await _context.CuentasContables
                            .FirstOrDefaultAsync(c => c.Categoria == "Activo" && 
                                                     c.Nombre.Contains("Corriente") && 
                                                     c.CuentaPadreId == categoriaActivo.Id);
                            
                        if (categoriaActivosCorrientes == null)
                        {
                            categoriaActivosCorrientes = new CuentaContable
                            {
                                Codigo = "1.1",
                                Nombre = "Activos Corrientes",
                                Descripcion = "Activos de corto plazo",
                                Categoria = "Activo",
                                Naturaleza = "Deudora",
                                TipoCuenta = "Mayor",
                                CuentaPadreId = categoriaActivo.Id,
                                Nivel = 2,
                                Orden = 1,
                                Activo = true,
                                EmpresaId = EmpresaId,
                                FechaCreacion = DateTime.UtcNow
                            };
                            _context.CuentasContables.Add(categoriaActivosCorrientes);
                            await _context.SaveChangesAsync();
                            
                            // Verificar que se guardó correctamente
                            if (categoriaActivosCorrientes.Id <= 0)
                            {
                                throw new Exception("No se pudo crear la categoría contable 'Activos Corrientes'.");
                            }
                        }
                        
                        // Buscar/Crear categoría Bancos
                        categoriaBancos = await _context.CuentasContables
                            .FirstOrDefaultAsync(c => c.Categoria == "Activo" && 
                                                     c.Nombre == "Bancos" && 
                                                     c.CuentaPadreId == categoriaActivosCorrientes.Id);
                            
                        if (categoriaBancos == null)
                        {
                            categoriaBancos = new CuentaContable
                            {
                                Codigo = "1.1.01",
                                Nombre = "Bancos",
                                Descripcion = "Cuentas bancarias de la empresa",
                                Categoria = "Activo",
                                Naturaleza = "Deudora",
                                TipoCuenta = "Mayor",
                                UsoCuenta = "Bancos",
                                CuentaPadreId = categoriaActivosCorrientes.Id,
                                Nivel = 3,
                                Orden = 1,
                                Activo = true,
                                EmpresaId = EmpresaId,
                                FechaCreacion = DateTime.UtcNow
                            };
                            _context.CuentasContables.Add(categoriaBancos);
                            await _context.SaveChangesAsync();
                            
                            // Verificar que se guardó correctamente
                            if (categoriaBancos.Id <= 0)
                            {
                                throw new Exception("No se pudo crear la categoría contable 'Bancos'.");
                            }
                        }
                        
                        // 2. Crear una cuenta contable específica para este banco
                        string codigoBase = categoriaBancos.Codigo ?? "1.1.01";
                        int ultimoDigito = 1;
                        
                        // Buscar el último código utilizado para generar uno secuencial
                        var ultimaCuentaBanco = await _context.CuentasContables
                            .Where(c => c.CuentaPadreId == categoriaBancos.Id)
                            .OrderByDescending(c => c.Codigo)
                            .FirstOrDefaultAsync();
                            
                        if (ultimaCuentaBanco != null && ultimaCuentaBanco.Codigo != null && ultimaCuentaBanco.Codigo.StartsWith(codigoBase))
                        {
                            try
                            {
                                // Tratar de obtener el último número secuencial
                                string ultimaParteCodigo = ultimaCuentaBanco.Codigo?.Substring(codigoBase.Length) ?? string.Empty;
                                if (!string.IsNullOrEmpty(ultimaParteCodigo) && ultimaParteCodigo.StartsWith("."))
                                {
                                    string ultimoNumero = ultimaParteCodigo.Substring(1);
                                    if (int.TryParse(ultimoNumero, out int numero))
                                    {
                                        ultimoDigito = numero + 1;
                                    }
                                }
                            }
                            catch
                            {
                                // Si hay algún error al procesar el código, simplemente usamos el valor predeterminado
                                ultimoDigito = 1;
                            }
                        }
                        
                        // Generar un nombre único para la cuenta contable
                        string nombreCuentaContable = $"Banco - {viewModel.EntidadBancaria} - {viewModel.Nombre}";
                        // Truncar si es necesario para cumplir con restricciones de longitud
                        if (nombreCuentaContable.Length > 100)
                        {
                            nombreCuentaContable = nombreCuentaContable.Substring(0, 97) + "...";
                        }
                        
                        // Crear la cuenta contable específica para este banco
                        cuentaContableBanco = new CuentaContable
                        {
                            Codigo = $"{codigoBase}.{ultimoDigito:D2}",
                            Nombre = nombreCuentaContable,
                            Descripcion = $"Cuenta bancaria {viewModel.NumeroCuenta} en {viewModel.EntidadBancaria}",
                            Categoria = "Activo",
                            Naturaleza = "Deudora",
                            TipoCuenta = "Movimiento",
                            UsoCuenta = "Bancos",
                            CuentaPadreId = categoriaBancos.Id,
                            Nivel = 4,
                            Orden = ultimoDigito,
                            Activo = true,
                            EmpresaId = EmpresaId,
                            FechaCreacion = DateTime.UtcNow
                        };
                        
                        _context.CuentasContables.Add(cuentaContableBanco);
                        await _context.SaveChangesAsync();
                        
                        // Verificar que la cuenta contable esté creada correctamente
                        if (cuentaContableBanco.Id <= 0)
                        {
                            throw new Exception("No se pudo crear la cuenta contable para el banco.");
                        }
                        
                        // 3. Crear el banco con su cuenta contable asociada
                        var banco = new Banco
                        {
                            Nombre = viewModel.Nombre,
                            NumeroCuenta = viewModel.NumeroCuenta,
                            TipoCuenta = viewModel.TipoCuenta,
                            EntidadBancaria = viewModel.EntidadBancaria,
                            Moneda = viewModel.Moneda,
                            Descripcion = viewModel.Descripcion,
                            SaldoInicial = viewModel.SaldoInicial,
                            SaldoActual = viewModel.SaldoInicial, // Al crear, el saldo actual es igual al inicial
                            SaldoConciliado = 0, // Inicializamos el saldo conciliado en 0
                            FechaApertura = viewModel.FechaApertura,
                            Activo = viewModel.Activo,
                            CuentaContableId = cuentaContableBanco.Id, // Asignar la cuenta contable específica
                            EmpresaId = EmpresaId,
                            FechaCreacion = DateTime.UtcNow,
                            UsuarioCreacion = User.Identity?.Name ?? string.Empty
                        };
                        
                        _context.Bancos.Add(banco);
                        await _context.SaveChangesAsync();
                        
                        // Confirmar la transacción
                        await transaction.CommitAsync();
                        
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        // Revertir la transacción en caso de error
                        await transaction.RollbackAsync();
                        
                        // Log error y mostrar mensaje al usuario
                        ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}");
                        
                        // Para depuración
                        System.Diagnostics.Debug.WriteLine($"Error detallado: {ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                ModelState.AddModelError(string.Empty, $"Error general: {ex.Message}");
                // Para depuración
                System.Diagnostics.Debug.WriteLine($"Error general detallado: {ex}");
            }
            
            return View(viewModel);
        }

        // GET: Banco/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banco = await _context.Bancos
                .Include(b => b.CuentaContable)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (banco == null)
            {
                return NotFound();
            }
            
            var viewModel = new BancoViewModel
            {
                Id = banco.Id,
                Nombre = banco.Nombre,
                NumeroCuenta = banco.NumeroCuenta,
                TipoCuenta = banco.TipoCuenta,
                EntidadBancaria = banco.EntidadBancaria,
                Moneda = banco.Moneda,
                Descripcion = banco.Descripcion,
                SaldoInicial = banco.SaldoInicial,
                SaldoActual = banco.SaldoActual,
                SaldoConciliado = banco.SaldoConciliado,
                FechaApertura = banco.FechaApertura,
                Activo = banco.Activo,
                CuentaContableId = banco.CuentaContableId,
                CuentaContableNombre = banco.CuentaContable?.Nombre
            };
            
            // Obtener cuentas contables de tipo Bancos
            var cuentasBancarias = await _context.CuentasContables
                .Where(c => c.UsoCuenta == "Bancos" && c.TipoCuenta == "Movimiento" || c.Id == banco.CuentaContableId)
                .OrderBy(c => c.Codigo)
                .ToListAsync();
                
            ViewBag.CuentasContables = new SelectList(cuentasBancarias, "Id", "Nombre", banco.CuentaContableId);
            
            // Obtener lista de monedas de los países para el dropdown
            var monedas = DataLists.LatinAmericanCountries
                .Select(c => new { Value = c.Currency, Text = $"{c.Currency} - {c.CurrencyName}" })
                .Distinct()
                .OrderBy(c => c.Text)
                .ToList();
                
            ViewBag.Monedas = new SelectList(monedas, "Value", "Text", banco.Moneda);
            
            return View(viewModel);
        }

        // POST: Banco/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BancoViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var banco = await _context.Bancos.FindAsync(id);
                    
                    if (banco == null)
                    {
                        return NotFound();
                    }
                    
                    // Calcular el nuevo saldo actual si el saldo inicial cambia
                    decimal diferenciaSaldo = viewModel.SaldoInicial - banco.SaldoInicial;
                    
                    banco.Nombre = viewModel.Nombre;
                    banco.NumeroCuenta = viewModel.NumeroCuenta;
                    banco.TipoCuenta = viewModel.TipoCuenta;
                    banco.EntidadBancaria = viewModel.EntidadBancaria;
                    banco.Moneda = viewModel.Moneda;
                    banco.Descripcion = viewModel.Descripcion;
                    banco.SaldoInicial = viewModel.SaldoInicial;
                    banco.SaldoActual = banco.SaldoActual + diferenciaSaldo; // Ajustar el saldo actual
                    banco.FechaApertura = viewModel.FechaApertura;
                    banco.Activo = viewModel.Activo;
                    banco.CuentaContableId = viewModel.CuentaContableId;
                    banco.FechaModificacion = DateTime.UtcNow;
                    banco.UsuarioModificacion = User.Identity?.Name ?? string.Empty;
                    
                    _context.Update(banco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BancoExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            // Si el modelo no es válido, recargar los dropdown lists
            var cuentasBancarias = await _context.CuentasContables
                .Where(c => c.UsoCuenta == "Bancos" && c.TipoCuenta == "Movimiento" || c.Id == viewModel.CuentaContableId)
                .OrderBy(c => c.Codigo)
                .ToListAsync();
                
            ViewBag.CuentasContables = new SelectList(cuentasBancarias, "Id", "Nombre", viewModel.CuentaContableId);
            
            var monedas = DataLists.LatinAmericanCountries
                .Select(c => new { Value = c.Currency, Text = $"{c.Currency} - {c.CurrencyName}" })
                .Distinct()
                .OrderBy(c => c.Text)
                .ToList();
                
            ViewBag.Monedas = new SelectList(monedas, "Value", "Text", viewModel.Moneda);
            
            return View(viewModel);
        }

        // GET: Banco/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banco = await _context.Bancos
                .Include(b => b.CuentaContable)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (banco == null)
            {
                return NotFound();
            }
            
            // Verificar si hay transacciones asociadas
            var tieneTransacciones = await _context.TransaccionesBanco
                .AnyAsync(t => t.BancoId == id);
                
            ViewBag.TieneTransacciones = tieneTransacciones;

            return View(banco);
        }

        // POST: Banco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banco = await _context.Bancos.FindAsync(id);
            
            if (banco == null)
            {
                return NotFound();
            }
            
            // Verificar si hay transacciones asociadas
            var tieneTransacciones = await _context.TransaccionesBanco
                .AnyAsync(t => t.BancoId == id);
                
            if (tieneTransacciones)
            {
                ModelState.AddModelError(string.Empty, "No se puede eliminar esta cuenta bancaria porque tiene transacciones asociadas.");
                return View(banco);
            }
            
            _context.Bancos.Remove(banco);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BancoExists(int id)
        {
            return await _context.Bancos.AnyAsync(e => e.Id == id);
        }
        
        // GET: Banco/Transacciones/5
        public async Task<IActionResult> Transacciones(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banco = await _context.Bancos
                .Include(b => b.CuentaContable)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (banco == null)
            {
                return NotFound();
            }
            
            var transacciones = await _context.TransaccionesBanco
                .Where(t => t.BancoId == id)
                .Include(t => t.Contacto)
                .Include(t => t.BancoDestino)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();
                
            ViewBag.Banco = banco;
            
            return View(transacciones);
        }
        
        // GET: Banco/NuevaTransaccion/5
        public async Task<IActionResult> NuevaTransaccion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banco = await _context.Bancos
                .Include(b => b.CuentaContable)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (banco == null)
            {
                return NotFound();
            }
            
            var viewModel = new TransaccionBancariaViewModel
            {
                BancoId = banco.Id,
                BancoNombre = banco.Nombre,
                Fecha = DateTime.Now
            };
            
            // Obtener otros bancos para transferencias
            var bancos = await _context.Bancos
                .Where(b => b.Id != id && b.Activo)
                .OrderBy(b => b.Nombre)
                .ToListAsync();
                
            ViewBag.BancosDestino = new SelectList(bancos, "Id", "Nombre");
            
            // Obtener contactos para asociar a la transacción
            var contactos = await _context.Contactos
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
                
            ViewBag.Contactos = new SelectList(contactos, "Id", "Nombre");
            
            ViewBag.Banco = banco;
            
            return View(viewModel);
        }
        
        // POST: Banco/NuevaTransaccion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NuevaTransaccion(TransaccionBancariaViewModel viewModel)
        {
            // Obtener el ID de empresa usando el servicio
            var EmpresaId = await _empresaService.ObtenerEmpresaActualId();
            
            if (ModelState.IsValid)
            {
                var banco = await _context.Bancos.FindAsync(viewModel.BancoId);
                
                if (banco == null)
                {
                    return NotFound();
                }
                
                var transaccion = new TransaccionBanco
                {
                    BancoId = viewModel.BancoId,
                    Tipo = MapTipoTransaccion(viewModel.Tipo),
                    Monto = viewModel.Monto,
                    Fecha = viewModel.Fecha,
                    Concepto = viewModel.Concepto,
                    Referencia = viewModel.Referencia,
                    Descripcion = viewModel.Descripcion,
                    ContactoId = viewModel.ContactoId,
                    BancoDestinoId = viewModel.BancoDestinoId,
                    EmpresaId = EmpresaId,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = User.Identity?.Name ?? string.Empty
                };
                
                _context.Add(transaccion);
                
                // Actualizar el saldo de la cuenta bancaria
                switch (viewModel.Tipo)
                {
                    case TipoTransaccionBancaria.Deposito:
                    case TipoTransaccionBancaria.CobroCliente:
                        banco.SaldoActual += viewModel.Monto;
                        break;
                    case TipoTransaccionBancaria.Retiro:
                    case TipoTransaccionBancaria.PagoProveedor:
                    case TipoTransaccionBancaria.GastoBancario:
                    case TipoTransaccionBancaria.Cheque:
                        banco.SaldoActual -= viewModel.Monto;
                        break;
                    case TipoTransaccionBancaria.Transferencia:
                        banco.SaldoActual -= viewModel.Monto;
                        
                        // Si es una transferencia, actualizar el saldo de la cuenta destino
                        if (viewModel.BancoDestinoId.HasValue)
                        {
                            var bancoDestino = await _context.Bancos.FindAsync(viewModel.BancoDestinoId.Value);
                            
                            if (bancoDestino != null)
                            {
                                bancoDestino.SaldoActual += viewModel.Monto;
                                
                                // Crear la transacción espejo en la cuenta destino
                                var transaccionDestino = new TransaccionBanco
                                {
                                    BancoId = viewModel.BancoDestinoId.Value,
                                    Tipo = TipoTransaccionBancaria.Ingreso,
                                    Monto = viewModel.Monto,
                                    Fecha = viewModel.Fecha,
                                    Concepto = $"Transferencia desde {banco.Nombre}",
                                    Referencia = viewModel.Referencia,
                                    Descripcion = viewModel.Descripcion,
                                    ContactoId = viewModel.ContactoId,
                                    BancoDestinoId = viewModel.BancoId, // Referencia a la cuenta de origen
                                    EmpresaId = EmpresaId,
                                    FechaCreacion = DateTime.UtcNow,
                                    UsuarioCreacion = User.Identity?.Name ?? string.Empty
                                };
                                
                                _context.Add(transaccionDestino);
                            }
                        }
                        break;
                    case TipoTransaccionBancaria.Ajuste:
                        // Para ajustes, el monto puede ser positivo o negativo
                        banco.SaldoActual += viewModel.Monto;
                        break;
                }
                
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Transacciones), new { id = viewModel.BancoId });
            }
            
            // Si el modelo no es válido, recargar los dropdown lists
            var banco2 = await _context.Bancos.FindAsync(viewModel.BancoId);
            ViewBag.Banco = banco2;
            
            var bancos = await _context.Bancos
                .Where(b => b.Id != viewModel.BancoId && b.Activo)
                .OrderBy(b => b.Nombre)
                .ToListAsync();
                
            ViewBag.BancosDestino = new SelectList(bancos, "Id", "Nombre", viewModel.BancoDestinoId);
            
            var contactos = await _context.Contactos
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
                
            ViewBag.Contactos = new SelectList(contactos, "Id", "Nombre", viewModel.ContactoId);
            
            return View(viewModel);
        }
        
        // Método auxiliar para mapear tipos de transacción
        private TipoTransaccionBancaria MapTipoTransaccion(TipoTransaccionBancaria tipoViewModel)
        {
            return tipoViewModel;
        }
        
        // GET: Banco/Conciliacion
        public async Task<IActionResult> Conciliacion()
        {
            var bancos = await _context.Bancos
                .Include(b => b.CuentaContable)
                .Where(b => b.Activo)
                .OrderBy(b => b.Nombre)
                .ToListAsync();
                
            // Get the most recent conciliations for each bank
            var recentConciliations = await _context.ConciliacionesBancarias
                .GroupBy(c => c.BancoId)
                .Select(g => g.OrderByDescending(c => c.FechaConciliacion).FirstOrDefault())
                .ToListAsync();
                
            ViewBag.RecentConciliations = recentConciliations;
                
            return View(bancos);
        }
        
        // GET: Bancos/Transacciones - Para todas las cuentas
        public async Task<IActionResult> TransaccionesBancarias()
        {
            // Obtener las transacciones de los últimos 30 días de todas las cuentas
            var fechaInicio = DateTime.Now.AddDays(-30);
            
            var transacciones = await _context.TransaccionesBanco
                .Include(t => t.Banco)
                .Include(t => t.BancoDestino)
                .Include(t => t.Contacto)
                .Where(t => t.Fecha >= fechaInicio)
                .OrderByDescending(t => t.Fecha)
                .Take(100) // Limitar a 100 transacciones por rendimiento
                .ToListAsync();
                
            // Obtener todas las cuentas bancarias activas para el filtro
            var bancos = await _context.Bancos
                .Where(b => b.Activo)
                .OrderBy(b => b.Nombre)
                .ToListAsync();
                
            ViewBag.Bancos = bancos;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = DateTime.Now;
                
            return View(transacciones);
        }
    }
} 