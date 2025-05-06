using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories;

namespace personapi_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaApiController : ControllerBase
    {
        private readonly IPersonaRepository _repo;

        public PersonaApiController(IPersonaRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Persona
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var personas = await _repo.GetAllAsync();
            return Ok(personas);
        }

        // GET: api/Persona/123
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var persona = await _repo.GetByIdAsync(id);
            if (persona == null) return NotFound();
            return Ok(persona);
        }

        // POST: api/Persona
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Persona persona)
        {
            var existente = await _repo.GetByIdAsync(persona.Cc);
            if (existente != null)
                return Conflict("La cédula ya está registrada.");

            if (!new[] { "M", "m", "F", "f" }.Contains(persona.Genero))
                return BadRequest("El campo Género solo puede ser 'M' o 'F'.");

            await _repo.AddAsync(persona);
            return CreatedAtAction(nameof(GetById), new { id = persona.Cc }, persona);
        }

        // PUT: api/Persona/123
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Persona persona)
        {
            if (id != persona.Cc)
                return BadRequest("La cédula del cuerpo no coincide con la de la URL.");

            if (!new[] { "M", "m", "F", "f" }.Contains(persona.Genero))
                return BadRequest("El campo Género solo puede ser 'M' o 'F'.");

            if (!await _repo.ExistsAsync(id))
                return NotFound();

            await _repo.UpdateAsync(persona);
            return NoContent();
        }

        // DELETE: api/Persona/123
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var persona = await _repo.GetByIdAsync(id);
            if (persona == null) return NotFound();

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
