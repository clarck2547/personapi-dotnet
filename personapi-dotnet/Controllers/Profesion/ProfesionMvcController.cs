using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories;
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers
{
    public class ProfesionMvcController : Controller
    {
        private readonly IProfesionRepository _profesionRepository;

        public ProfesionMvcController(IProfesionRepository profesionRepository)
        {
            _profesionRepository = profesionRepository;
        }

        // GET: Profesions
        public async Task<IActionResult> Index()
        {
            var profesiones = await _profesionRepository.GetAllAsync();
            return View("~/Views/ProfesionMvc/Index.cshtml", profesiones);
        }

        // GET: Profesions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesion = await _profesionRepository.GetByIdAsync(id.Value);
            if (profesion == null)
            {
                return NotFound();
            }

            return View("~/Views/ProfesionMvc/Details.cshtml", profesion);
        }

        // GET: Profesions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profesions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Des")] Profesion profesion)
        {

            if (ModelState.IsValid)
            {
                await _profesionRepository.AddAsync(profesion);
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/ProfesionMvc/Create.cshtml", profesion);
        }

        // GET: Profesions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesion = await _profesionRepository.GetByIdAsync(id.Value);
            if (profesion == null)
            {
                return NotFound();
            }

            return View("~/Views/ProfesionMvc/Edit.cshtml", profesion);
        }

        // POST: Profesions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Des")] Profesion profesion)
        {
            if (id != profesion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _profesionRepository.UpdateAsync(profesion);
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/ProfesionMvc/Edit.cshtml", profesion);
        }

        // GET: Profesions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesion = await _profesionRepository.GetByIdAsync(id.Value);
            if (profesion == null)
            {
                return NotFound();
            }

            return View("~/Views/ProfesionMvc/Delete.cshtml", profesion);
        }

        // POST: Profesions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _profesionRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
