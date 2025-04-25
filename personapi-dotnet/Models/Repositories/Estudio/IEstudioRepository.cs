using System.Collections.Generic;
using System.Threading.Tasks;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public interface IEstudioRepository
    {
        Task<IEnumerable<Estudio>> GetAllAsync();
        Task<Estudio> GetByIdAsync(int idProf, int ccPer);
        Task AddAsync(Estudio estudio);
        Task UpdateAsync(Estudio estudio);
        Task DeleteAsync(int idProf, int ccPer);
        Task<List<Persona>> GetAllPersonasAsync();
        Task<List<Profesion>> GetAllProfesionesAsync();
        Task<Persona?> GetPersonaByIdAsync(int cc);
        Task<Profesion?> GetProfesionByIdAsync(int id);
        Task<bool> ExistsAsync(int idProf, int ccPer);
    }
}
