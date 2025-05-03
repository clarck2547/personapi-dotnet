using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories;
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelefonoApiController : ControllerBase
    {
        private readonly ITelefonoRepository _repo;

        public TelefonoApiController(ITelefonoRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Telefono
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var telefonos = await _repo.GetAllAsync();
            return Ok(telefonos);
        }

        // GET: api/Telefono/3001234567
        [HttpGet("{num}")]
        public async Task<IActionResult> GetById(string num)
        {
            var telefono = await _repo.GetByIdAsync(num);
            if (telefono == null) return NotFound();
            return Ok(telefono);
        }

        // POST: api/Telefono
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Telefono telefono)
        {
            // Verificar si ya existe
            var existente = await _repo.GetByIdAsync(telefono.Num);
            if (existente != null)
            {
                return Conflict("El número de teléfono ya está registrado.");
            }

            var persona = await _repo.GetPersonaByIdAsync(telefono.Duenio);
            if (persona == null)
            {
                return BadRequest("La persona asignada no existe.");
            }

            telefono.DuenioNavigation = persona;
            await _repo.AddAsync(telefono);
            return CreatedAtAction(nameof(GetById), new { num = telefono.Num }, telefono);
        }

        // PUT: api/Telefono/3001234567
        [HttpPut("{num}")]
        public async Task<IActionResult> Update(string num, [FromBody] Telefono telefono)
        {
            if (num != telefono.Num)
            {
                return BadRequest("El número de la URL no coincide con el del cuerpo.");
            }

            var persona = await _repo.GetPersonaByIdAsync(telefono.Duenio);
            if (persona == null)
            {
                return BadRequest("La persona asignada no existe.");
            }

            telefono.DuenioNavigation = persona;

            try
            {
                await _repo.UpdateAsync(telefono);
            }
            catch
            {
                if (!await _repo.ExistsAsync(num)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Telefono/3001234567
        [HttpDelete("{num}")]
        public async Task<IActionResult> Delete(string num)
        {
            var telefono = await _repo.GetByIdAsync(num);
            if (telefono == null) return NotFound();

            await _repo.DeleteAsync(num);
            return NoContent();
        }
    }
}
