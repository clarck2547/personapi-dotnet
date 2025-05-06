using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories;

namespace personapi_dotnet.Controllers
{
    public class PersonaMvcController : Controller
    {
        private readonly IPersonaRepository _repo;

        public PersonaMvcController(IPersonaRepository repo)
        {
            _repo = repo;
        }

        // GET: PersonaMvc
        public async Task<IActionResult> Index()
        {
            var personas = await _repo.GetAllAsync();
            return View("~/Views/PersonaMvc/Index.cshtml", personas);
        }

        // GET: PersonaMvc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var persona = await _repo.GetByIdAsync(id.Value);
            if (persona == null) return NotFound();

            return View("~/Views/PersonaMvc/Details.cshtml", persona);
        }

        // GET: PersonaMvc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PersonaMvc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona)
        {
            var personaExistente = await _repo.GetByIdAsync(persona.Cc);
            if (personaExistente != null)
            {
                ModelState.AddModelError("Cc", "Esta cédula ya está registrada. Por favor, ingrese una diferente.");
            }

            // Validar que el género sea M, m, F o f
            if (!new[] { "M", "m", "F", "f" }.Contains(persona.Genero))
            {
                ModelState.AddModelError("Genero", "El campo Género solo puede ser 'M' o 'F'.");
            }

            if (ModelState.IsValid)
            {
                await _repo.AddAsync(persona);
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/PersonaMvc/Create.cshtml", persona);
        }



        // GET: PersonaMvc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var persona = await _repo.GetByIdAsync(id.Value);
            if (persona == null) return NotFound();

            return View("~/Views/PersonaMvc/Edit.cshtml", persona);
        }

        // POST: PersonaMvc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona)
        {
            if (id != persona.Cc) return NotFound();

            // Validar que el género sea M, m, F o f
            if (!new[] { "M", "m", "F", "f" }.Contains(persona.Genero))
            {
                ModelState.AddModelError("Genero", "El campo Género solo puede ser 'M' o 'F'.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.UpdateAsync(persona);
                }
                catch
                {
                    if (!await _repo.ExistsAsync(persona.Cc))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/PersonaMvc/Edit.cshtml", persona);
        }

        // GET: PersonaMvc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var persona = await _repo.GetByIdAsync(id.Value);
            if (persona == null) return NotFound();

            return View("~/Views/PersonaMvc/Delete.cshtml", persona);
        }

        // POST: PersonaMvc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
