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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SistemaContable.Controllers
{
    public class BancoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BancoController(
            ApplicationDbContext context, 
            IEmpresaService empresaService,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _empresaService = empresaService;
            _webHostEnvironment = webHostEnvironment;
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
            var monedasList = DataLists.LatinAmericanCountries
                .Select(c => new { Value = c.Currency, Text = $"{c.Currency} - {c.CurrencyName}" })
                .Distinct()
                .OrderBy(c => c.Text)
                .ToList();
                
            ViewBag.Monedas = new SelectList(monedasList, "Value", "Text");
            
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
            
            if (ModelState.IsValid)
            {
                // Manejar la carga del logo si se ha proporcionado
                if (viewModel.LogoFile != null && viewModel.LogoFile.Length > 0)
                {
                    try
                    {
                        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "bancos");
                        if (!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }

                        var fileName = $"{Guid.NewGuid()}_{viewModel.LogoFile.FileName}";
                        var filePath = Path.Combine(uploads, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await viewModel.LogoFile.CopyToAsync(stream);
                        }

                        viewModel.LogoUrl = $"/uploads/bancos/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("LogoFile", $"Error al subir el logo: {ex.Message}");
                        
                        var monedasList = DataLists.LatinAmericanCountries
                            .Select(c => new { Value = c.Currency, Text = $"{c.Currency} - {c.CurrencyName}" })
                            .Distinct()
                            .OrderBy(c => c.Text)
                            .ToList();
                            
                        ViewBag.Monedas = new SelectList(monedasList, "Value", "Text");
                        
                        return View(viewModel);
                    }
                }
                
                // Iniciar una transacción para asegurar la integridad de los datos
                using var transaction = await _context.Database.BeginTransactionAsync();
                
                try
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
                    
                    // 1. Buscar la cuenta contable Bancos existente
                    var categoriaBancos = await _context.CuentasContables
                        .FirstOrDefaultAsync(c => c.Nombre == "Bancos" && c.UsoCuenta == "Bancos" && c.EmpresaId == EmpresaId);
                        
                    if (categoriaBancos == null)
                    {
                        // Si no existe la cuenta Bancos, mostrar un error y pedir que se configure primero el catálogo
                        ModelState.AddModelError(string.Empty, "No existe la cuenta contable 'Bancos' en el catálogo. Por favor, configure primero el catálogo de cuentas.");
                        return View(viewModel);
                    }
                    
                    // 2. Crear una cuenta contable específica para este banco bajo la cuenta Bancos existente
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
                    var cuentaContableBanco = new CuentaContable
                    {
                        Codigo = $"{codigoBase}.{ultimoDigito:D2}",
                        Nombre = nombreCuentaContable,
                        Descripcion = $"Cuenta bancaria {viewModel.NumeroCuenta} en {viewModel.EntidadBancaria}",
                        Categoria = "Activo",
                        Naturaleza = "Deudora",
                        TipoCuenta = "Movimiento",
                        UsoCuenta = "Bancos",
                        CuentaPadreId = categoriaBancos.Id,
                        Nivel = categoriaBancos.Nivel + 1, // Un nivel más que la cuenta padre
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
                        UsuarioCreacion = User.Identity?.Name ?? string.Empty,
                        LogoUrl = viewModel.LogoUrl // Asignar la URL del logo
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
            var monedasList = DataLists.LatinAmericanCountries
                .Select(c => new { Value = c.Currency, Text = $"{c.Currency} - {c.CurrencyName}" })
                .Distinct()
                .OrderBy(c => c.Text)
                .ToList();
                
            ViewBag.Monedas = new SelectList(monedasList, "Value", "Text", banco.Moneda);
            
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
                // Manejar la carga del logo si se ha proporcionado
                if (viewModel.LogoFile != null && viewModel.LogoFile.Length > 0)
                {
                    try
                    {
                        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "bancos");
                        if (!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }

                        var fileName = $"{Guid.NewGuid()}_{viewModel.LogoFile.FileName}";
                        var filePath = Path.Combine(uploads, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await viewModel.LogoFile.CopyToAsync(stream);
                        }

                        viewModel.LogoUrl = $"/uploads/bancos/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("LogoFile", $"Error al subir el logo: {ex.Message}");
                        
                        // Recargar los dropdown lists
                        var cuentasBancariasEdit = await _context.CuentasContables
                            .Where(c => c.UsoCuenta == "Bancos" && c.TipoCuenta == "Movimiento" || c.Id == viewModel.CuentaContableId)
                            .OrderBy(c => c.Codigo)
                            .ToListAsync();
                            
                        ViewBag.CuentasContables = new SelectList(cuentasBancariasEdit, "Id", "Nombre", viewModel.CuentaContableId);
                        
                        var monedasListEdit = DataLists.LatinAmericanCountries
                            .Select(c => new { Value = c.Currency, Text = $"{c.Currency} - {c.CurrencyName}" })
                            .Distinct()
                            .OrderBy(c => c.Text)
                            .ToList();
                            
                        ViewBag.Monedas = new SelectList(monedasListEdit, "Value", "Text", viewModel.Moneda);
                        
                        return View(viewModel);
                    }
                }
                
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
                    
                    // Actualizar LogoUrl solo si se ha seleccionado un archivo nuevo
                    if (!string.IsNullOrEmpty(viewModel.LogoUrl))
                    {
                        banco.LogoUrl = viewModel.LogoUrl;
                    }
                    
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
            
            var monedasList = DataLists.LatinAmericanCountries
                .Select(c => new { Value = c.Currency, Text = $"{c.Currency} - {c.CurrencyName}" })
                .Distinct()
                .OrderBy(c => c.Text)
                .ToList();
                
            ViewBag.Monedas = new SelectList(monedasList, "Value", "Text", viewModel.Moneda);
            
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
            
            // Obtener ID de empresa
            var EmpresaId = await _empresaService.ObtenerEmpresaActualId();
            
            // Obtener otros bancos para transferencias
            var bancos = await _context.Bancos
                .Where(b => b.Id != id && b.Activo && b.EmpresaId == EmpresaId)
                .OrderBy(b => b.Nombre)
                .ToListAsync();
            
            // Crear una lista combinada para el dropdown
            var cuentasDestino = new List<SelectListItem>();
            
            // Agregar los bancos
            if (bancos.Any())
            {
                cuentasDestino.Add(new SelectListItem { Value = "", Text = "-- CUENTAS BANCARIAS --", Disabled = true });
                foreach (var b in bancos)
                {
                    cuentasDestino.Add(new SelectListItem 
                    { 
                        Value = $"B{b.Id}", 
                        Text = $"{b.EntidadBancaria} - {b.Nombre}" 
                    });
                }
            }
            
            ViewBag.BancosDestino = cuentasDestino;
            
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
                    BancoDestinoId = null, // Se asignará después según el tipo de destino
                    EmpresaId = EmpresaId,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = User.Identity?.Name ?? string.Empty
                };
                
                // Procesar el ID de destino según el formato
                if (!string.IsNullOrEmpty(viewModel.BancoDestinoIdString))
                {
                    string destinoId = viewModel.BancoDestinoIdString;
                    
                    if (destinoId.StartsWith("B"))
                    {
                        // Es un banco
                        if (int.TryParse(destinoId.Substring(1), out int bancoId))
                        {
                            transaccion.BancoDestinoId = bancoId;
                            viewModel.BancoDestinoId = bancoId; // Asegurarnos de que el modelo tenga el valor
                        }
                    }
                    else if (destinoId.StartsWith("C"))
                    {
                        // Es una cuenta contable - la transferencia se procesará diferente
                        // (se registrará como movimiento contable)
                        if (int.TryParse(destinoId.Substring(1), out int cuentaId))
                        {
                            // Aquí podríamos implementar la lógica para transferencias a cuentas contables
                            // Por ahora, lo dejamos como ejemplo
                            transaccion.CuentaContableDestinoId = cuentaId;
                            viewModel.CuentaContableDestinoId = cuentaId; // Asignar al modelo
                        }
                    }
                }
                
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
                        
                        // Si es una transferencia a otro banco, actualizar el saldo de la cuenta destino
                        if (transaccion.BancoDestinoId.HasValue)
                        {
                            var bancoDestino = await _context.Bancos.FindAsync(transaccion.BancoDestinoId.Value);
                            
                            if (bancoDestino != null)
                            {
                                bancoDestino.SaldoActual += viewModel.Monto;
                                
                                // Crear la transacción espejo en la cuenta destino
                                var transaccionDestino = new TransaccionBanco
                                {
                                    BancoId = transaccion.BancoDestinoId.Value,
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
                        // Si es una transferencia a cuenta contable, implementar la lógica correspondiente
                        else if (transaccion.CuentaContableDestinoId.HasValue)
                        {
                            // Aquí implementaríamos el registro contable de la transferencia
                            // Por ahora sólo ajustamos el saldo del banco
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
            
            // Recargar las cuentas destino
            var EmpresaId2 = await _empresaService.ObtenerEmpresaActualId();
            
            var bancos = await _context.Bancos
                .Where(b => b.Id != viewModel.BancoId && b.Activo && b.EmpresaId == EmpresaId2)
                .OrderBy(b => b.Nombre)
                .ToListAsync();
            
            var cuentasDestino = new List<SelectListItem>();
            
            if (bancos.Any())
            {
                cuentasDestino.Add(new SelectListItem { Value = "", Text = "-- CUENTAS BANCARIAS --", Disabled = true });
                foreach (var b in bancos)
                {
                    cuentasDestino.Add(new SelectListItem 
                    { 
                        Value = $"B{b.Id}", 
                        Text = $"{b.EntidadBancaria} - {b.Nombre}",
                        Selected = viewModel.BancoDestinoIdString == $"B{b.Id}"
                    });
                }
            }
            
            ViewBag.BancosDestino = cuentasDestino;
            
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

        // POST: Banco/SubirLogo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubirLogo(int id, IFormFile file)
        {
            if (id <= 0 || file == null || file.Length <= 0)
            {
                return BadRequest("No se recibió un archivo válido.");
            }

            var banco = await _context.Bancos.FindAsync(id);
            if (banco == null)
            {
                return NotFound();
            }

            try
            {
                var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "bancos");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                banco.LogoUrl = $"/uploads/bancos/{fileName}";
                _context.Update(banco);
                await _context.SaveChangesAsync();

                return Ok(new { url = banco.LogoUrl });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al subir el logo: {ex.Message}");
            }
        }
    }
} 