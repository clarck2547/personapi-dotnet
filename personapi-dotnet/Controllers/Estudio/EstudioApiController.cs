using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories;

namespace personapi_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudioApiController : ControllerBase
    {
        private readonly IEstudioRepository _repo;

        public EstudioApiController(IEstudioRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Estudio
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var estudios = await _repo.GetAllAsync();
            return Ok(estudios);
        }

        // GET: api/Estudio/1/123
        [HttpGet("{idProf:int}/{ccPer:int}")]
        public async Task<IActionResult> GetById(int idProf, int ccPer)
        {
            var estudio = await _repo.GetByIdAsync(idProf, ccPer);
            if (estudio == null) return NotFound();
            return Ok(estudio);
        }

        // POST: api/Estudio
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Estudio estudio)
        {
            var persona = await _repo.GetPersonaByIdAsync(estudio.CcPer);
            var profesion = await _repo.GetProfesionByIdAsync(estudio.IdProf);

            if (persona == null || profesion == null)
                return BadRequest("Persona o Profesión no encontrada");

            estudio.CcPerNavigation = persona;
            estudio.IdProfNavigation = profesion;

            await _repo.AddAsync(estudio);
            return CreatedAtAction(nameof(GetById), new { idProf = estudio.IdProf, ccPer = estudio.CcPer }, estudio);
        }

        // PUT: api/Estudio/1/123
        [HttpPut("{idProf:int}/{ccPer:int}")]
        public async Task<IActionResult> Update(int idProf, int ccPer, [FromBody] Estudio estudio)
        {
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer)
                return BadRequest("IDs no coinciden");

            var persona = await _repo.GetPersonaByIdAsync(estudio.CcPer);
            var profesion = await _repo.GetProfesionByIdAsync(estudio.IdProf);

            if (persona == null || profesion == null)
                return BadRequest("Persona o Profesión no encontrada");

            estudio.CcPerNavigation = persona;
            estudio.IdProfNavigation = profesion;

            await _repo.UpdateAsync(estudio);
            return NoContent();
        }

        // DELETE: api/Estudio/1/123
        [HttpDelete("{idProf:int}/{ccPer:int}")]
        public async Task<IActionResult> Delete(int idProf, int ccPer)
        {
            var estudio = await _repo.GetByIdAsync(idProf, ccPer);
            if (estudio == null) return NotFound();

            await _repo.DeleteAsync(idProf, ccPer);
            return NoContent();
        }
    }
}
