using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories;
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesionApiController : ControllerBase
    {
        private readonly IProfesionRepository _profesionRepository;

        public ProfesionApiController(IProfesionRepository profesionRepository)
        {
            _profesionRepository = profesionRepository;
        }

        // GET: api/Profesion
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profesiones = await _profesionRepository.GetAllAsync();
            return Ok(profesiones);
        }

        // GET: api/Profesion/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profesion = await _profesionRepository.GetByIdAsync(id);
            if (profesion == null) return NotFound();

            return Ok(profesion);
        }

        // POST: api/Profesion
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Profesion profesion)
        {
            await _profesionRepository.AddAsync(profesion);
            return CreatedAtAction(nameof(GetById), new { id = profesion.Id }, profesion);
        }

        // PUT: api/Profesion/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Profesion profesion)
        {
            if (id != profesion.Id) return BadRequest("El ID de la URL no coincide con el del cuerpo.");

            var existente = await _profesionRepository.GetByIdAsync(id);
            if (existente == null) return NotFound();

            await _profesionRepository.UpdateAsync(profesion);
            return NoContent();
        }

        // DELETE: api/Profesion/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var profesion = await _profesionRepository.GetByIdAsync(id);
            if (profesion == null) return NotFound();

            await _profesionRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
