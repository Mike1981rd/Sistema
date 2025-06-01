using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContable.Data;
using SistemaContable.Models;
using SistemaContable.Services;
using SistemaContable.ViewModels;

namespace SistemaContable.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaService _empresaService;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, ApplicationDbContext context, IEmpresaService empresaService)
        {
            _authService = authService;
            _logger = logger;
            _context = context;
            _empresaService = empresaService;
        }

        // GET: Auth/Login
        public IActionResult Login(string? returnUrl = null)
        {
            // Si ya está autenticado, redirigir al dashboard
            if (_authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            _logger.LogInformation("POST Login iniciado - LoginIdentifier: {LoginIdentifier}", model?.LoginIdentifier ?? "NULL");
            
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state inválido. Errores: {Errors}", 
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))));
                return View(model);
            }

            try
            {
                var usuario = await _authService.AuthenticateAsync(model.LoginIdentifier, model.Password);

                if (usuario == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                    return View(model);
                }

                // Login exitoso
                await _authService.LoginUserAsync(usuario);

                _logger.LogInformation("Login exitoso para usuario: {UserId}", usuario.Id);

                // Redirigir a la URL de retorno o al dashboard
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el proceso de login");
                ModelState.AddModelError(string.Empty, "Ocurrió un error durante el proceso de autenticación. Intente nuevamente.");
                return View(model);
            }
        }

        // POST: Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutUserAsync();
            return RedirectToAction("Login");
        }

        // GET: Auth/Logout (para links directos)
        public async Task<IActionResult> LogoutGet()
        {
            await _authService.LogoutUserAsync();
            return RedirectToAction("Login");
        }

        // GET: Auth/Debug - Para verificar usuarios en desarrollo
        public async Task<IActionResult> Debug()
        {
            return View("DebugUsers");
        }

        // GET: Auth/GetUsers - API para obtener usuarios
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Usuarios
                    .Select(u => new
                    {
                        u.Id,
                        u.NombreCompleto,
                        u.NombreUsuario,
                        u.Email,
                        u.Activo,
                        u.EmpresaId,
                        u.RolId
                    })
                    .ToListAsync();

                return Json(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return Json(new { error = ex.Message });
            }
        }

        // POST: Auth/CreateTestUser - Crear usuario de prueba
        [HttpPost]
        public async Task<IActionResult> CreateTestUser([FromBody] CreateTestUserRequest request)
        {
            try
            {
                var empresaId = await _empresaService.ObtenerEmpresaActualId();
                
                // Verificar si ya existe
                var existeUsuario = await _context.Usuarios
                    .AnyAsync(u => u.NombreUsuario == request.NombreUsuario || u.Email == request.Email);

                if (existeUsuario)
                {
                    return Json(new { success = false, message = "Ya existe un usuario con ese nombre o email" });
                }

                // Buscar un rol existente o crear uno básico
                var rol = await _context.Roles.FirstOrDefaultAsync(r => r.EmpresaId == empresaId && r.Activo);
                if (rol == null)
                {
                    // Crear rol básico
                    rol = new Rol
                    {
                        Nombre = "Administrador",
                        Descripcion = "Rol de administrador",
                        EmpresaId = empresaId,
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        Permisos = new List<string>()
                    };
                    _context.Roles.Add(rol);
                    await _context.SaveChangesAsync();
                }

                var usuario = new Usuario
                {
                    NombreCompleto = request.NombreCompleto,
                    NombreUsuario = request.NombreUsuario,
                    Email = request.Email,
                    PasswordHash = _authService.HashPassword(request.Password),
                    EmpresaId = empresaId,
                    RolId = rol.Id,
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Usuario creado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario de prueba");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Auth/HashPassword - Generar hash de contraseña
        [HttpPost]
        public IActionResult HashPassword([FromBody] HashPasswordRequest request)
        {
            try
            {
                var hash = _authService.HashPassword(request.Password);
                return Json(new { hash = hash });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public class CreateTestUserRequest
        {
            public string NombreCompleto { get; set; } = string.Empty;
            public string NombreUsuario { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class HashPasswordRequest
        {
            public string Password { get; set; } = string.Empty;
        }
    }
}