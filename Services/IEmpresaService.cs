using System.Threading.Tasks;

namespace SistemaContable.Services
{
    public interface IEmpresaService
    {
        Task<int> ObtenerEmpresaActualId();
    }
} 