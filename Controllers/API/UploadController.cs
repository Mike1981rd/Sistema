using Microsoft.AspNetCore.Mvc;
using SistemaContable.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IEmpresaService _empresaService;

        public UploadController(IWebHostEnvironment hostEnvironment, IEmpresaService empresaService)
        {
            _hostEnvironment = hostEnvironment;
            _empresaService = empresaService;
        }

        [HttpPost("imagen")]
        public async Task<IActionResult> SubirImagen()
        {
            try
            {
                var archivo = Request.Form.Files["imagen"];
                var tipo = Request.Form["tipo"].ToString();

                if (archivo == null || archivo.Length == 0)
                {
                    return BadRequest(new { success = false, mensaje = "No se seleccionó ningún archivo" });
                }

                // Validar tipo de archivo
                var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
                
                if (!Array.Exists(extensionesPermitidas, ext => ext == extension))
                {
                    return BadRequest(new { success = false, mensaje = "Tipo de archivo no permitido. Use: " + string.Join(", ", extensionesPermitidas) });
                }

                // Validar tamaño (2MB máximo)
                if (archivo.Length > 2 * 1024 * 1024)
                {
                    return BadRequest(new { success = false, mensaje = "El archivo es muy grande. Máximo 2MB" });
                }

                // Determinar carpeta según el tipo
                string carpeta;
                switch (tipo?.ToLower())
                {
                    case "producto":
                        carpeta = "items";
                        break;
                    case "usuario":
                        carpeta = "usuarios";
                        break;
                    case "proveedor":
                        carpeta = "proveedores";
                        break;
                    case "cliente":
                        carpeta = "clientes";
                        break;
                    case "banco":
                        carpeta = "bancos";
                        break;
                    default:
                        carpeta = "general";
                        break;
                }

                // Crear directorio si no existe
                var uploadsPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", carpeta);
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Generar nombre único para el archivo
                var nombreArchivo = Guid.NewGuid().ToString() + extension;
                var rutaCompleta = Path.Combine(uploadsPath, nombreArchivo);

                // Guardar el archivo
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                // Devolver la URL relativa
                var urlRelativa = $"/uploads/{carpeta}/{nombreArchivo}";

                return Ok(new { 
                    success = true, 
                    url = urlRelativa,
                    mensaje = "Imagen subida correctamente",
                    nombreOriginal = archivo.FileName,
                    nombreGuardado = nombreArchivo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    mensaje = "Error interno del servidor: " + ex.Message 
                });
            }
        }

        [HttpDelete("imagen")]
        public IActionResult EliminarImagen([FromQuery] string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return BadRequest(new { success = false, mensaje = "URL no proporcionada" });
                }

                // Convertir URL relativa a ruta física
                var rutaRelativa = url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                var rutaCompleta = Path.Combine(_hostEnvironment.WebRootPath, rutaRelativa);

                if (System.IO.File.Exists(rutaCompleta))
                {
                    System.IO.File.Delete(rutaCompleta);
                    return Ok(new { success = true, mensaje = "Imagen eliminada correctamente" });
                }
                else
                {
                    return NotFound(new { success = false, mensaje = "Archivo no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    mensaje = "Error al eliminar imagen: " + ex.Message 
                });
            }
        }
    }
}