using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaContable.Services;
using System.Threading.Tasks;

namespace SistemaContable.Helpers
{
    public static class PermisosViewHelper
    {
        /// <summary>
        /// Helper para verificar permisos en las vistas Razor
        /// </summary>
        /// <param name="htmlHelper">HTML Helper</param>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="permiso">Permiso a verificar</param>
        /// <param name="content">Contenido a mostrar si tiene el permiso</param>
        /// <returns>Contenido HTML si tiene el permiso, vacío si no</returns>
        public static async Task<IHtmlContent> SiTienePermisoAsync(this IHtmlHelper htmlHelper, int usuarioId, string permiso, IHtmlContent content)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;
            var permisosService = (IPermisosService)httpContext.RequestServices.GetService(typeof(IPermisosService));
            
            if (permisosService == null)
                return HtmlString.Empty;

            var tienePermiso = await permisosService.TienePermisoAsync(usuarioId, permiso);
            
            return tienePermiso ? content : HtmlString.Empty;
        }

        /// <summary>
        /// Helper para verificar acceso a módulos en las vistas Razor
        /// </summary>
        /// <param name="htmlHelper">HTML Helper</param>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="modulo">Módulo a verificar</param>
        /// <param name="content">Contenido a mostrar si tiene acceso</param>
        /// <returns>Contenido HTML si tiene acceso, vacío si no</returns>
        public static async Task<IHtmlContent> SiTieneAccesoModuloAsync(this IHtmlHelper htmlHelper, int usuarioId, string modulo, IHtmlContent content)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;
            var permisosService = (IPermisosService)httpContext.RequestServices.GetService(typeof(IPermisosService));
            
            if (permisosService == null)
                return HtmlString.Empty;

            var tieneAcceso = await permisosService.TieneAccesoModuloAsync(usuarioId, modulo);
            
            return tieneAcceso ? content : HtmlString.Empty;
        }

        /// <summary>
        /// Helper para verificar si puede ver elementos del menú
        /// </summary>
        /// <param name="htmlHelper">HTML Helper</param>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="seccionMenu">Sección del menú</param>
        /// <param name="content">Contenido a mostrar si puede ver</param>
        /// <returns>Contenido HTML si puede ver, vacío si no</returns>
        public static async Task<IHtmlContent> SiPuedeVerMenuAsync(this IHtmlHelper htmlHelper, int usuarioId, string seccionMenu, IHtmlContent content)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;
            var permisosService = (IPermisosService)httpContext.RequestServices.GetService(typeof(IPermisosService));
            
            if (permisosService == null)
                return HtmlString.Empty;

            var puedeVer = await permisosService.PuedeVerMenuAsync(usuarioId, seccionMenu);
            
            return puedeVer ? content : HtmlString.Empty;
        }
    }
}