using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;
using System.Security.Cryptography;
using System.Text;

namespace SistemaContable.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsuariosController(ApplicationDbContext context, IEmpresaService empresaService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _empresaService = empresaService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index(string tab)
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                return RedirectToAction("Index", "Empresas");
            }

            bool activos = string.IsNullOrEmpty(tab) || tab == "Activos";
            ViewBag.Tab = activos ? "Activos" : "Inactivos";

            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.EmpresaId == empresaId && u.Activo == activos)
                .OrderBy(u => u.NombreCompleto)
                .ToListAsync();

            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id && u.EmpresaId == empresaId);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public async Task<IActionResult> Create()
        {
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            await CargarRolesSelectList(empresaId);
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreCompleto,NombreUsuario,RolId,PinPOS,Telefono,Email,Direccion,Ciudad,EstadoProvincia,CodigoPostal")] Usuario usuario, string password, string confirmPassword, IFormFile? foto)
        {
            // Debug logging
            Console.WriteLine($"=== CREATE USUARIO - INICIO ===");
            Console.WriteLine($"NombreCompleto: {usuario.NombreCompleto}");
            Console.WriteLine($"NombreUsuario: {usuario.NombreUsuario}");
            Console.WriteLine($"RolId: {usuario.RolId}");
            Console.WriteLine($"Password Length: {password?.Length ?? 0}");
            
            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            if (empresaId <= 0)
            {
                TempData["ErrorMessage"] = "Debe seleccionar una empresa";
                return RedirectToAction("Index", "Empresas");
            }

            // Validar contraseñas
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "La contraseña es requerida");
            }
            else if (password.Length < 6)
            {
                ModelState.AddModelError("password", "La contraseña debe tener al menos 6 caracteres");
            }
            
            if (password != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Las contraseñas no coinciden");
            }

            // Verificar si el nombre de usuario ya existe
            var existeUsuario = await _context.Usuarios
                .AnyAsync(u => u.NombreUsuario == usuario.NombreUsuario && u.EmpresaId == empresaId);

            if (existeUsuario)
            {
                ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya existe");
            }
            
            // Remover errores de PasswordHash del ModelState ya que lo asignamos manualmente
            ModelState.Remove("PasswordHash");

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.EmpresaId = empresaId;
                    usuario.PasswordHash = HashPassword(password);
                    usuario.FechaCreacion = DateTime.UtcNow;
                    usuario.FechaModificacion = DateTime.UtcNow;
                    usuario.Activo = true;
                    
                    // Debug: verificar el estado del objeto antes de guardar
                    Console.WriteLine($"=== ANTES DE GUARDAR ===");
                    Console.WriteLine($"Usuario.Id: {usuario.Id}");
                    Console.WriteLine($"Usuario.EmpresaId: {usuario.EmpresaId}");
                    Console.WriteLine($"Usuario.PasswordHash Length: {usuario.PasswordHash.Length}");
                    Console.WriteLine($"Usuario.FechaCreacion: {usuario.FechaCreacion:yyyy-MM-dd HH:mm:ss}");

                    // Manejar la foto de perfil
                    if (foto != null && foto.Length > 0)
                    {
                        try
                        {
                            var fileName = await GuardarFotoPerfil(foto);
                            usuario.FotoUrl = fileName;
                        }
                        catch (InvalidOperationException ex)
                        {
                            ModelState.AddModelError("foto", ex.Message);
                            await CargarRolesSelectList(empresaId, usuario.RolId);
                            return View(usuario);
                        }
                    }

                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = $"Usuario '{usuario.NombreCompleto}' creado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log detallado del error
                    Console.WriteLine($"Error al crear usuario: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                    }
                    
                    ModelState.AddModelError("", "Error al crear el usuario: " + ex.Message);
                    TempData["ErrorMessage"] = "Error al crear el usuario. Por favor, intente nuevamente.";
                }
            }
            else
            {
                // Log de errores de validación
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error de validación: {modelError.ErrorMessage}");
                }
            }

            await CargarRolesSelectList(empresaId, usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.EmpresaId == empresaId);

            if (usuario == null)
            {
                return NotFound();
            }

            await CargarRolesSelectList(empresaId, usuario.RolId);
            ViewBag.CambiarPassword = false;
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreCompleto,NombreUsuario,RolId,PinPOS,Telefono,Email,Direccion,Ciudad,EstadoProvincia,CodigoPostal,Activo")] Usuario usuario, string? password, string? confirmPassword, IFormFile? foto, bool cambiarPassword)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            var empresaId = await _empresaService.ObtenerEmpresaActualId();
            var usuarioOriginal = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id && u.EmpresaId == empresaId);

            if (usuarioOriginal == null)
            {
                return NotFound();
            }

            // Validar contraseñas si se está cambiando
            if (cambiarPassword)
            {
                if (string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("Password", "La contraseña es requerida");
                }
                else if (password != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden");
                }
            }

            // Verificar si el nombre de usuario ya existe (excluyendo el actual)
            var existeUsuario = await _context.Usuarios
                .AnyAsync(u => u.NombreUsuario == usuario.NombreUsuario && 
                              u.EmpresaId == empresaId && 
                              u.Id != id);

            if (existeUsuario)
            {
                ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya existe");
            }
            
            // Remover errores de PasswordHash del ModelState ya que lo manejamos manualmente
            ModelState.Remove("PasswordHash");

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.EmpresaId = empresaId;
                    usuario.FechaCreacion = usuarioOriginal.FechaCreacion;
                    usuario.FechaModificacion = DateTime.UtcNow;

                    // Mantener la contraseña original si no se está cambiando
                    if (!cambiarPassword)
                    {
                        usuario.PasswordHash = usuarioOriginal.PasswordHash;
                    }
                    else
                    {
                        usuario.PasswordHash = HashPassword(password!);
                    }

                    // Manejar la foto de perfil
                    if (foto != null && foto.Length > 0)
                    {
                        // Eliminar foto anterior si existe
                        if (!string.IsNullOrEmpty(usuarioOriginal.FotoUrl))
                        {
                            EliminarFotoPerfil(usuarioOriginal.FotoUrl);
                        }
                        var fileName = await GuardarFotoPerfil(foto);
                        usuario.FotoUrl = fileName;
                    }
                    else
                    {
                        usuario.FotoUrl = usuarioOriginal.FotoUrl;
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Usuario actualizado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            await CargarRolesSelectList(empresaId, usuario.RolId);
            ViewBag.CambiarPassword = cambiarPassword;
            return View(usuario);
        }

        // POST: Usuarios/ToggleEstado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            try
            {
                Console.WriteLine($"=== ToggleEstado llamado con ID: {id} ===");
                
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == id && u.EmpresaId == empresaId);

                if (usuario != null)
                {
                    usuario.Activo = !usuario.Activo;
                    usuario.FechaModificacion = DateTime.UtcNow;

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"El usuario {usuario.NombreCompleto} ha sido {(usuario.Activo ? "activado" : "desactivado")} correctamente";
                    Console.WriteLine($"Usuario {usuario.NombreCompleto} actualizado correctamente");
                }
                else
                {
                    Console.WriteLine($"Usuario con ID {id} no encontrado");
                    TempData["ErrorMessage"] = "Usuario no encontrado";
                }

                return RedirectToAction(nameof(Index), new { tab = usuario?.Activo == true ? "Activos" : "Inactivos" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ToggleEstado: {ex.Message}");
                TempData["ErrorMessage"] = "Error al actualizar el estado del usuario";
                return RedirectToAction(nameof(Index));
            }
        }

        // Métodos auxiliares
        private async Task CargarRolesSelectList(int empresaId, int? selectedRolId = null)
        {
            var roles = await _context.Roles
                .Where(r => r.EmpresaId == empresaId && r.Activo)
                .OrderBy(r => r.Nombre)
                .Select(r => new { r.Id, r.Nombre })
                .ToListAsync();

            ViewBag.RolId = new SelectList(roles, "Id", "Nombre", selectedRolId);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private async Task<string> GuardarFotoPerfil(IFormFile foto)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "usuarios");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Validar tamaño de archivo (máximo 5MB)
            if (foto.Length > 5 * 1024 * 1024)
            {
                throw new InvalidOperationException("La imagen no puede superar los 5MB");
            }

            // Validar tipo de archivo
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(foto.ContentType.ToLower()))
            {
                throw new InvalidOperationException("Solo se permiten imágenes JPG, PNG o GIF");
            }

            var extension = Path.GetExtension(foto.FileName);
            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await foto.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        private void EliminarFotoPerfil(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "usuarios", fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
    }
}