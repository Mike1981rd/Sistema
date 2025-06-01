using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaContable.Services
{
    public interface IPermisosService
    {
        /// <summary>
        /// Verifica si el usuario actual tiene un permiso específico
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="permiso">Clave del permiso a verificar</param>
        /// <returns>True si tiene el permiso, False si no</returns>
        Task<bool> TienePermisoAsync(int usuarioId, string permiso);

        /// <summary>
        /// Obtiene todos los permisos activos del usuario
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <returns>Lista de permisos del usuario</returns>
        Task<List<string>> ObtenerPermisosUsuarioAsync(int usuarioId);

        /// <summary>
        /// Verifica si el usuario actual tiene acceso a un módulo específico
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="modulo">Nombre del módulo (ej: "configuracion", "contabilidad")</param>
        /// <returns>True si tiene acceso al módulo, False si no</returns>
        Task<bool> TieneAccesoModuloAsync(int usuarioId, string modulo);

        /// <summary>
        /// Obtiene la estructura de permisos filtrada según los permisos del usuario
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <returns>Estructura de permisos filtrada</returns>
        Task<Dictionary<string, Dictionary<string, string>>> ObtenerPermisosDisponiblesAsync(int usuarioId);

        /// <summary>
        /// Verifica si el usuario puede ver una sección específica del menú
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="seccionMenu">Sección del menú a verificar</param>
        /// <returns>True si puede ver la sección, False si no</returns>
        Task<bool> PuedeVerMenuAsync(int usuarioId, string seccionMenu);
    }
}