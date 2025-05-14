using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaContable.Controllers
{
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            // TODO: Obtener empresa del usuario
            int empresaId = 1;

            var compras = await _context.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.Almacen)
                .Include(c => c.PlazoPago)
                .Where(c => c.EmpresaId == empresaId)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();

            return View(compras);
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // TODO: Obtener empresa del usuario
            int empresaId = 1;

            var compra = await _context.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.Almacen)
                .Include(c => c.PlazoPago)
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.Item)
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.Impuesto)
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.UnidadMedida)
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);

            if (compra == null)
            {
                return NotFound();
            }

            var viewModel = MapCompraToViewModel(compra);
            
            await PrepareViewBagsForCreateEdit(viewModel);

            return View(viewModel);
        }

        // GET: Compras/Create
        public async Task<IActionResult> Create()
        {
            // TODO: Obtener empresa del usuario
            int empresaId = 1;

            var viewModel = new CompraViewModel
            {
                Fecha = DateTime.Now,
                EmpresaId = empresaId,
                Estado = "Pendiente",
                Numero = await GenerateNextNumberAsync(empresaId)
            };

            await PrepareViewBagsForCreateEdit(viewModel);

            return View(viewModel);
        }

        // POST: Compras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompraViewModel viewModel)
        {
            // TODO: Obtener empresa del usuario
            int empresaId = 1;
            viewModel.EmpresaId = empresaId;

            if (ModelState.IsValid)
            {
                try
                {
                    var compra = new Compra
                    {
                        Numero = viewModel.Numero,
                        Fecha = viewModel.Fecha,
                        ProveedorId = viewModel.ProveedorId,
                        AlmacenId = viewModel.AlmacenId,
                        Referencia = viewModel.Referencia,
                        Observaciones = viewModel.Observaciones,
                        PlazoPagoId = viewModel.PlazoPagoId,
                        FechaVencimiento = viewModel.FechaVencimiento,
                        Subtotal = viewModel.Subtotal,
                        Descuento = viewModel.Descuento,
                        Impuestos = viewModel.Impuestos,
                        Total = viewModel.Total,
                        Estado = viewModel.Estado,
                        EmpresaId = empresaId,
                        FechaCreacion = DateTime.Now
                    };

                    _context.Add(compra);
                    await _context.SaveChangesAsync();

                    // Procesar los detalles
                    if (viewModel.Detalles != null && viewModel.Detalles.Any())
                    {
                        foreach (var detalleVM in viewModel.Detalles)
                        {
                            var detalle = new CompraDetalle
                            {
                                CompraId = compra.Id,
                                ItemId = detalleVM.ItemId,
                                Descripcion = detalleVM.Descripcion,
                                Cantidad = detalleVM.Cantidad,
                                Precio = detalleVM.Precio,
                                Subtotal = detalleVM.Subtotal,
                                PorcentajeDescuento = detalleVM.PorcentajeDescuento,
                                MontoDescuento = detalleVM.MontoDescuento,
                                ImpuestoId = detalleVM.ImpuestoId,
                                MontoImpuesto = detalleVM.MontoImpuesto,
                                Total = detalleVM.Total,
                                UnidadMedidaId = detalleVM.UnidadMedidaId,
                                FactorConversion = detalleVM.FactorConversion,
                                EmpresaId = empresaId,
                                FechaCreacion = DateTime.Now
                            };

                            _context.Add(detalle);
                        }

                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al crear la compra: {ex.Message}");
                }
            }

            await PrepareViewBagsForCreateEdit(viewModel);
            return View(viewModel);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // TODO: Obtener empresa del usuario
            int empresaId = 1;

            var compra = await _context.Compras
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.Item)
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.Impuesto)
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.UnidadMedida)
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);

            if (compra == null)
            {
                return NotFound();
            }

            // No permitir editar compras recibidas o anuladas
            if (compra.Estado != "Pendiente")
            {
                return RedirectToAction(nameof(Details), new { id = compra.Id });
            }

            var viewModel = MapCompraToViewModel(compra);

            await PrepareViewBagsForCreateEdit(viewModel);

            return View(viewModel);
        }

        // POST: Compras/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompraViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            // TODO: Obtener empresa del usuario
            int empresaId = 1;
            viewModel.EmpresaId = empresaId;

            if (ModelState.IsValid)
            {
                try
                {
                    var compra = await _context.Compras
                        .Include(c => c.Detalles)
                        .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);

                    if (compra == null)
                    {
                        return NotFound();
                    }

                    // No permitir editar compras recibidas o anuladas
                    if (compra.Estado != "Pendiente")
                    {
                        return RedirectToAction(nameof(Details), new { id = compra.Id });
                    }

                    // Actualizar los datos de la cabecera
                    compra.Numero = viewModel.Numero;
                    compra.Fecha = viewModel.Fecha;
                    compra.ProveedorId = viewModel.ProveedorId;
                    compra.AlmacenId = viewModel.AlmacenId;
                    compra.Referencia = viewModel.Referencia;
                    compra.Observaciones = viewModel.Observaciones;
                    compra.PlazoPagoId = viewModel.PlazoPagoId;
                    compra.FechaVencimiento = viewModel.FechaVencimiento;
                    compra.Subtotal = viewModel.Subtotal;
                    compra.Descuento = viewModel.Descuento;
                    compra.Impuestos = viewModel.Impuestos;
                    compra.Total = viewModel.Total;
                    compra.FechaModificacion = DateTime.Now;

                    // Obtener los IDs de los detalles actuales para identificar los eliminados
                    var detallesActualesIds = compra.Detalles.Select(d => d.Id).ToList();
                    var detallesNuevosIds = viewModel.Detalles.Where(d => d.Id > 0).Select(d => d.Id).ToList();

                    // Identificar detalles a eliminar (están en actuales pero no en nuevos)
                    var detallesAEliminar = detallesActualesIds.Except(detallesNuevosIds).ToList();

                    // Eliminar detalles
                    foreach (var detalleId in detallesAEliminar)
                    {
                        var detalle = compra.Detalles.FirstOrDefault(d => d.Id == detalleId);
                        if (detalle != null)
                        {
                            _context.ComprasDetalles.Remove(detalle);
                        }
                    }

                    // Actualizar o agregar detalles
                    foreach (var detalleVM in viewModel.Detalles)
                    {
                        if (detalleVM.Id > 0) // Actualizar detalle existente
                        {
                            var detalle = compra.Detalles.FirstOrDefault(d => d.Id == detalleVM.Id);
                            if (detalle != null)
                            {
                                detalle.ItemId = detalleVM.ItemId;
                                detalle.Descripcion = detalleVM.Descripcion;
                                detalle.Cantidad = detalleVM.Cantidad;
                                detalle.Precio = detalleVM.Precio;
                                detalle.Subtotal = detalleVM.Subtotal;
                                detalle.PorcentajeDescuento = detalleVM.PorcentajeDescuento;
                                detalle.MontoDescuento = detalleVM.MontoDescuento;
                                detalle.ImpuestoId = detalleVM.ImpuestoId;
                                detalle.MontoImpuesto = detalleVM.MontoImpuesto;
                                detalle.Total = detalleVM.Total;
                                detalle.UnidadMedidaId = detalleVM.UnidadMedidaId;
                                detalle.FactorConversion = detalleVM.FactorConversion;
                                detalle.FechaModificacion = DateTime.Now;
                            }
                        }
                        else // Agregar nuevo detalle
                        {
                            var detalle = new CompraDetalle
                            {
                                CompraId = compra.Id,
                                ItemId = detalleVM.ItemId,
                                Descripcion = detalleVM.Descripcion,
                                Cantidad = detalleVM.Cantidad,
                                Precio = detalleVM.Precio,
                                Subtotal = detalleVM.Subtotal,
                                PorcentajeDescuento = detalleVM.PorcentajeDescuento,
                                MontoDescuento = detalleVM.MontoDescuento,
                                ImpuestoId = detalleVM.ImpuestoId,
                                MontoImpuesto = detalleVM.MontoImpuesto,
                                Total = detalleVM.Total,
                                UnidadMedidaId = detalleVM.UnidadMedidaId,
                                FactorConversion = detalleVM.FactorConversion,
                                EmpresaId = empresaId,
                                FechaCreacion = DateTime.Now
                            };

                            _context.Add(detalle);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al editar la compra: {ex.Message}");
                }
            }

            await PrepareViewBagsForCreateEdit(viewModel);
            return View(viewModel);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // TODO: Obtener empresa del usuario
            int empresaId = 1;

            var compra = await _context.Compras
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);

            if (compra == null)
            {
                return NotFound();
            }

            // En lugar de eliminar, cambiamos el estado a "Anulado"
            compra.Estado = "Anulado";
            compra.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Compras/Recibir/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recibir(int id)
        {
            // TODO: Obtener empresa del usuario
            int empresaId = 1;

            var compra = await _context.Compras
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.Item)
                .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId);

            if (compra == null)
            {
                return NotFound();
            }

            if (compra.Estado != "Pendiente")
            {
                return BadRequest("La compra no está en estado Pendiente");
            }

            // Cambiar estado de la compra
            compra.Estado = "Recibido";
            compra.FechaModificacion = DateTime.Now;

            // TODO: Actualizar el inventario
            // TODO: Crear entrada de diario si es necesario

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = compra.Id });
        }

        // Métodos helper
        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.Id == id);
        }

        private async Task<string> GenerateNextNumberAsync(int empresaId)
        {
            // Generar el próximo número de compra
            var ultimaCompra = await _context.Compras
                .Where(c => c.EmpresaId == empresaId)
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync();

            int numero = 1;
            if (ultimaCompra != null)
            {
                if (int.TryParse(ultimaCompra.Numero.Replace("CO-", ""), out int ultimoNumero))
                {
                    numero = ultimoNumero + 1;
                }
            }

            return $"CO-{numero:D6}";
        }

        private CompraViewModel MapCompraToViewModel(Compra compra)
        {
            var viewModel = new CompraViewModel
            {
                Id = compra.Id,
                Numero = compra.Numero,
                Fecha = compra.Fecha,
                ProveedorId = compra.ProveedorId,
                ProveedorNombre = compra.Proveedor?.NombreRazonSocial,
                AlmacenId = compra.AlmacenId,
                AlmacenNombre = compra.Almacen?.Nombre,
                Referencia = compra.Referencia,
                Observaciones = compra.Observaciones,
                PlazoPagoId = compra.PlazoPagoId,
                PlazoPagoNombre = compra.PlazoPago?.Nombre,
                FechaVencimiento = compra.FechaVencimiento,
                Subtotal = compra.Subtotal,
                Descuento = compra.Descuento,
                Impuestos = compra.Impuestos,
                Total = compra.Total,
                Estado = compra.Estado,
                EntradaDiarioId = compra.EntradaDiarioId,
                EmpresaId = compra.EmpresaId,
                Detalles = new List<CompraDetalleViewModel>()
            };

            // Mapear los detalles
            if (compra.Detalles != null && compra.Detalles.Any())
            {
                foreach (var detalle in compra.Detalles)
                {
                    viewModel.Detalles.Add(new CompraDetalleViewModel
                    {
                        Id = detalle.Id,
                        CompraId = detalle.CompraId,
                        ItemId = detalle.ItemId,
                        ItemNombre = detalle.Item?.Nombre,
                        ItemCodigo = detalle.Item?.Codigo,
                        Descripcion = detalle.Descripcion,
                        Cantidad = detalle.Cantidad,
                        Precio = detalle.Precio,
                        Subtotal = detalle.Subtotal,
                        PorcentajeDescuento = detalle.PorcentajeDescuento,
                        MontoDescuento = detalle.MontoDescuento,
                        ImpuestoId = detalle.ImpuestoId,
                        ImpuestoNombre = detalle.Impuesto?.Nombre,
                        PorcentajeImpuesto = detalle.Impuesto?.Porcentaje ?? 0,
                        MontoImpuesto = detalle.MontoImpuesto,
                        Total = detalle.Total,
                        UnidadMedidaId = detalle.UnidadMedidaId,
                        UnidadMedidaNombre = detalle.UnidadMedida?.Nombre,
                        FactorConversion = detalle.FactorConversion,
                        EmpresaId = detalle.EmpresaId
                    });
                }
            }

            return viewModel;
        }

        private async Task PrepareViewBagsForCreateEdit(CompraViewModel viewModel)
        {
            // TODO: Obtener empresa del usuario
            int empresaId = 1;

            // Proveedores
            viewModel.Proveedores = new SelectList(
                await _context.Clientes
                    .Where(c => c.EsProveedor && c.EmpresaId == empresaId)
                    .OrderBy(c => c.NombreRazonSocial)
                    .ToListAsync(),
                "Id", "NombreRazonSocial", viewModel.ProveedorId);

            // Almacenes
            viewModel.Almacenes = new SelectList(
                await _context.Almacenes
                    .Where(a => a.EmpresaId == empresaId)
                    .OrderBy(a => a.Nombre)
                    .ToListAsync(),
                "Id", "Nombre", viewModel.AlmacenId);

            // Plazos de pago
            viewModel.PlazosPago = new SelectList(
                await _context.PlazosPago
                    .OrderBy(p => p.Nombre)
                    .ToListAsync(),
                "Id", "Nombre", viewModel.PlazoPagoId);

            // Impuestos
            viewModel.ImpuestosSelectList = new SelectList(
                await _context.Impuestos
                    .Where(i => i.EmpresaId == empresaId && i.Activo)
                    .OrderBy(i => i.Nombre)
                    .ToListAsync(),
                "Id", "Nombre");

            // Unidades de medida
            viewModel.UnidadesMedida = new SelectList(
                await _context.UnidadesMedida
                    .OrderBy(u => u.Nombre)
                    .ToListAsync(),
                "Id", "Nombre");

            // Items (Productos)
            ViewBag.Items = await _context.Items
                .Where(i => i.EmpresaId == empresaId && i.Activo)
                .OrderBy(i => i.Nombre)
                .Select(i => new
                {
                    i.Id,
                    i.Nombre,
                    i.Codigo,
                    i.UnidadMedidaInventarioId
                })
                .ToListAsync();
        }

        // API Endpoints para AJAX
        [HttpGet]
        public async Task<IActionResult> GetItemInfo(int itemId)
        {
            var item = await _context.Items
                .Include(i => i.UnidadMedidaInventario)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item == null)
            {
                return NotFound();
            }

            // Buscar información del proveedor específico (si se proporciona)
            decimal precioCompra = 0;
            var proveedorId = Request.Query["proveedorId"].ToString();
            int proveedorIdInt = 0;

            if (int.TryParse(proveedorId, out proveedorIdInt) && proveedorIdInt > 0)
            {
                var itemProveedor = await _context.ItemProveedores
                    .FirstOrDefaultAsync(ip => ip.ItemId == itemId && ip.ProveedorId == proveedorIdInt);

                if (itemProveedor != null)
                {
                    precioCompra = itemProveedor.PrecioCompra;
                }
            }

            // Si no hay precio específico, usar el costo estándar
            if (precioCompra <= 0)
            {
                precioCompra = item.CostoEstandar;
            }

            return Json(new
            {
                item.Id,
                item.Nombre,
                item.Codigo,
                Descripcion = item.Descripcion ?? item.Nombre,
                PrecioCompra = precioCompra,
                UnidadMedidaId = item.UnidadMedidaInventarioId,
                UnidadMedidaNombre = item.UnidadMedidaInventario?.Nombre,
                ImpuestoId = item.ImpuestoId,
                FactorConversion = 1
            });
        }
    }
} 