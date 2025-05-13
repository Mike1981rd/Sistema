using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SistemaContable.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var diagnostico = new
            {
                timestamp = DateTime.UtcNow,
                status = "ok",
                version = GetType().Assembly.GetName().Version?.ToString() ?? "desconocida",
                memoria = Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024) + " MB",
                entorno = new Dictionary<string, string>
                {
                    { "runtime", Environment.Version.ToString() },
                    { "os", Environment.OSVersion.ToString() },
                    { "processors", Environment.ProcessorCount.ToString() }
                }
            };

            return Ok(diagnostico);
        }
    }
} 