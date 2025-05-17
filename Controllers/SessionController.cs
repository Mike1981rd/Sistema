using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace SistemaContable.Controllers
{
    public class SessionController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /Session/SetEmpresa?id=4
        public IActionResult SetEmpresa(int id)
        {
            if (_httpContextAccessor.HttpContext?.Session != null)
            {
                var empresaIdBytes = BitConverter.GetBytes(id);
                _httpContextAccessor.HttpContext.Session.Set("EmpresaId", empresaIdBytes);
                
                TempData["Success"] = $"EmpresaId establecido a {id}";
                return RedirectToAction("Index", "Home");
            }
            
            TempData["Error"] = "No se pudo establecer la sesión";
            return RedirectToAction("Index", "Home");
        }
        
        // GET: /Session/Current
        public IActionResult Current()
        {
            int? empresaId = null;
            
            if (_httpContextAccessor.HttpContext?.Session != null)
            {
                if (_httpContextAccessor.HttpContext.Session.TryGetValue("EmpresaId", out byte[]? empresaIdBytes) && 
                    empresaIdBytes != null)
                {
                    empresaId = BitConverter.ToInt32(empresaIdBytes, 0);
                }
            }
            
            ViewBag.CurrentEmpresaId = empresaId;
            return View();
        }
    }
}