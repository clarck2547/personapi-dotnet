using System.Collections.Generic;
using System.Threading.Tasks;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public interface ITelefonoRepository
    {
        Task<List<Telefono>> GetAllAsync();
        Task<Telefono?> GetByIdAsync(string num);
        Task AddAsync(Telefono telefono);
        Task UpdateAsync(Telefono telefono);
        Task DeleteAsync(string num);
        Task<bool> ExistsAsync(string num);
        Task<Persona?> GetPersonaByIdAsync(int cc);
        Task<List<Persona>> GetAllPersonasAsync();
    }
}
