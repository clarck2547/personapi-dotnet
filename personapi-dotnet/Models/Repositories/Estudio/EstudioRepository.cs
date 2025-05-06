using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace personapi_dotnet.Repositories
{
    public class EstudioRepository : IEstudioRepository
    {
        private readonly PersonaDbContext _context;

        public EstudioRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estudio>> GetAllAsync()
        {
            return await _context.Estudios
                .Include(e => e.CcPerNavigation)
                .Include(e => e.IdProfNavigation)
                .ToListAsync();
        }

        public async Task<Estudio> GetByIdAsync(int idProf, int ccPer)
        {
            return await _context.Estudios
                .Include(e => e.CcPerNavigation)
                .Include(e => e.IdProfNavigation)
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
        }

        public async Task AddAsync(Estudio estudio)
        {
            _context.Add(estudio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Estudio estudio)
        {
            _context.Update(estudio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int idProf, int ccPer)
        {
            var estudio = await _context.Estudios.FindAsync(idProf, ccPer);
            if (estudio != null)
            {
                _context.Estudios.Remove(estudio);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Persona>> GetAllPersonasAsync()
        {
            return await _context.Personas.ToListAsync();
        }
        public async Task<List<Profesion>> GetAllProfesionesAsync()
        {
            return await _context.Profesions.ToListAsync();
        }
        public async Task<Persona?> GetPersonaByIdAsync(int cc)
        {
            return await _context.Personas.FindAsync(cc);
        }
        public async Task<Profesion?> GetProfesionByIdAsync(int id)
        {
            return await _context.Profesions.FindAsync(id);
        }
        public async Task<bool> ExistsAsync(int idProf, int ccPer)
        {
            return await _context.Estudios.AnyAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
        }
    }
}
