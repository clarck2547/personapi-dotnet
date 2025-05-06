using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories;
using personapi_dotnet.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
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
            var dtoList = profesiones.Select(p => new ProfesionDTO
            {
                Id = p.Id,
                Nom = p.Nom,
                Des = p.Des
            });

            return Ok(dtoList);
        }

        // GET: api/Profesion/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profesion = await _profesionRepository.GetByIdAsync(id);
            if (profesion == null) return NotFound();

            var dto = new ProfesionDTO
            {
                Id = profesion.Id,
                Nom = profesion.Nom,
                Des = profesion.Des
            };

            return Ok(dto);
        }

        // POST: api/Profesion
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProfesionDTO dto)
        {
            var profesion = new Profesion
            {
                Id = dto.Id,
                Nom = dto.Nom,
                Des = dto.Des
            };

            await _profesionRepository.AddAsync(profesion);

            return CreatedAtAction(nameof(GetById), new { id = profesion.Id }, dto);
        }

        // PUT: api/Profesion/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProfesionDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID de la URL no coincide con el del cuerpo.");

            var existente = await _profesionRepository.GetByIdAsync(id);
            if (existente == null) return NotFound();

            var profesion = new Profesion
            {
                Id = dto.Id,
                Nom = dto.Nom,
                Des = dto.Des,
                Estudios = existente.Estudios // mantener la relación existente
            };

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
