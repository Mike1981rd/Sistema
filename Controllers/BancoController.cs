using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.ViewModels;

namespace SistemaContable.Controllers
{
    public class BancoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BancoController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create()
        {
            // Obtener cuentas contables de tipo Bancos
            var cuentasBancarias = await _context.CuentasContables
                .Where(c => c.UsoCuenta == "Bancos" && c.TipoCuenta == "Movimiento")
                .OrderBy(c => c.Codigo)
                .ToListAsync();
                
            ViewBag.CuentasContables = new SelectList(cuentasBancarias, "Id", "Nombre");
            
            // Obtener lista de monedas de los países para el dropdown
            var monedas = DataLists.LatinAmericanCountries
                .Select(c => new { Value = c.Currency, Text = $"{c.Currency} - {c.CurrencyName}" })
                .Distinct()
                .OrderBy(c => c.Text)
                .ToList();
                
            ViewBag.Monedas = new SelectList(monedas, "Value", "Text");
            
            return View();
        }

        // POST: Banco/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BancoViewModel viewModel)
        {
            // Hardcodear la empresa por ahora, luego se obtendrá de la sesión
            const int EmpresaId = 1;
            
            if (ModelState.IsValid)
            {
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
                    FechaApertura = viewModel.FechaApertura,
                    Activo = viewModel.Activo,
                    CuentaContableId = viewModel.CuentaContableId,
                    EmpresaId = EmpresaId,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = User.Identity?.Name
                };
                
                _context.Add(banco);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            
            // Si el modelo no es válido, recargar los dropdown lists
            var cuentasBancarias = await _context.CuentasContables
                .Where(c => c.UsoCuenta == "Bancos" && c.TipoCuenta == "Movimiento")
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
                    banco.UsuarioModificacion = User.Identity?.Name;
                    
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
            // Hardcodear la empresa por ahora, luego se obtendrá de la sesión
            const int EmpresaId = 1;
            
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
                    UsuarioCreacion = User.Identity?.Name
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
                                    UsuarioCreacion = User.Identity?.Name
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