using SistemaContable.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaContable.Repositories.Interfaces
{
    public interface IContactoRepository
    {
        Task<IEnumerable<Contacto>> GetAllAsync();
        Task<Contacto> GetByIdAsync(int id);
        Task<Contacto> CreateAsync(Contacto contacto);
        Task UpdateAsync(Contacto contacto);
        Task DeleteAsync(int id);
        Task<bool> ExisteIdentificacionAsync(string identificacion, int empresaId);
        Task<IEnumerable<Contacto>> BuscarPorNombreAsync(string termino);
        Task<IEnumerable<Contacto>> GetClientesAsync(int empresaId);
        Task<IEnumerable<Contacto>> GetProveedoresAsync(int empresaId);
    }
} 