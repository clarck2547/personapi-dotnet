using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using personapi_dotnet.Models.DTOs;

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
            var dtoList = telefonos.Select(t => new TelefonoDTO
            {
                Num = t.Num,
                Oper = t.Oper,
                Duenio = t.Duenio
            });

            return Ok(dtoList);
        }

        // GET: api/Telefono/3001234567
        [HttpGet("{num}")]
        public async Task<IActionResult> GetById(string num)
        {
            var telefono = await _repo.GetByIdAsync(num);
            if (telefono == null) return NotFound();

            var dto = new TelefonoDTO
            {
                Num = telefono.Num,
                Oper = telefono.Oper,
                Duenio = telefono.Duenio
            };

            return Ok(dto);
        }

        // POST: api/Telefono
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TelefonoDTO dto)
        {
            var existente = await _repo.GetByIdAsync(dto.Num);
            if (existente != null)
                return Conflict("El número de teléfono ya está registrado.");

            var persona = await _repo.GetPersonaByIdAsync(dto.Duenio);
            if (persona == null)
                return BadRequest("La persona asignada no existe.");

            var telefono = new Telefono
            {
                Num = dto.Num,
                Oper = dto.Oper,
                Duenio = dto.Duenio,
                DuenioNavigation = persona
            };

            await _repo.AddAsync(telefono);

            return CreatedAtAction(nameof(GetById), new { num = dto.Num }, dto);
        }

        // PUT: api/Telefono/3001234567
        [HttpPut("{num}")]
        public async Task<IActionResult> Update(string num, [FromBody] TelefonoDTO dto)
        {
            if (num != dto.Num)
                return BadRequest("El número de la URL no coincide con el del cuerpo.");

            var persona = await _repo.GetPersonaByIdAsync(dto.Duenio);
            if (persona == null)
                return BadRequest("La persona asignada no existe.");

            var telefono = new Telefono
            {
                Num = dto.Num,
                Oper = dto.Oper,
                Duenio = dto.Duenio,
                DuenioNavigation = persona
            };

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
