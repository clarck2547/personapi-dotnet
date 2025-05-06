using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace personapi_dotnet.Repositories
{
    public class ProfesionRepository : IProfesionRepository
    {
        private readonly PersonaDbContext _context;

        public ProfesionRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profesion>> GetAllAsync()
        {
            return await _context.Profesions.ToListAsync();
        }

        public async Task<Profesion> GetByIdAsync(int id)
        {
            return await _context.Profesions.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Profesion profesion)
        {
            // Verificamos si el id de la profesion ya existe
            bool idExiste = await _context.Profesions.AnyAsync(p => p.Id == profesion.Id);

            if (idExiste)
            {
                // Lanza una excepción si el id ya existe
                throw new Exception("El id proporcionado ya existe. Por favor, elija otro.");
            }

            // Si el id no existe, agregamos la nueva profesion
            _context.Add(profesion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Profesion profesion)
        {
            _context.Update(profesion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var profesion = await _context.Profesions.FindAsync(id);
            if (profesion != null)
            {
                _context.Profesions.Remove(profesion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
