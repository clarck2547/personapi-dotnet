using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories;

namespace personapi_dotnet.Controllers
{
    public class TelefonoMvcController : Controller
    {
        private readonly ITelefonoRepository _repo;

        public TelefonoMvcController(ITelefonoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var telefonos = await _repo.GetAllAsync();
            return View("~/Views/TelefonoMvc/Index.cshtml", telefonos);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var telefono = await _repo.GetByIdAsync(id);
            if (telefono == null) return NotFound();

            return View("~/Views/TelefonoMvc/Details.cshtml", telefono);
        }

        public async Task<IActionResult> Create()
        {
            var personas = await _repo.GetAllPersonasAsync();
            ViewData["Duenio"] = new SelectList(personas, "Cc", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            // Verificar si el número ya existe
            var telefonoExistente = await _repo.GetByIdAsync(telefono.Num);
            if (telefonoExistente != null)
            {
                // Mostrar mensaje de error y recargar formulario con datos
                ModelState.AddModelError("Num", "Este número de teléfono ya está registrado. Por favor, ingrese uno diferente.");

                var personasLista = await _repo.GetAllPersonasAsync();
                ViewData["Duenio"] = new SelectList(personasLista, "Cc", "Nombre", telefono.Duenio);
                return View("~/Views/TelefonoMvc/Create.cshtml", telefono);
            }

            // Verificar si la persona existe
            var persona = await _repo.GetPersonaByIdAsync(telefono.Duenio);
            if (persona != null)
            {
                telefono.DuenioNavigation = persona;
                await _repo.AddAsync(telefono);
                return RedirectToAction(nameof(Index));
            }

            var personas = await _repo.GetAllPersonasAsync();
            ViewData["Duenio"] = new SelectList(personas, "Cc", "Nombre", telefono.Duenio);
            return View("~/Views/TelefonoMvc/Create.cshtml", telefono);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var telefono = await _repo.GetByIdAsync(id);
            if (telefono == null) return NotFound();

            var personas = await _repo.GetAllPersonasAsync();
            ViewData["Duenio"] = new SelectList(personas, "Cc", "Nombre", telefono.Duenio);
            return View("~/Views/TelefonoMvc/Edit.cshtml", telefono);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            if (id != telefono.Num) return NotFound();

            var persona = await _repo.GetPersonaByIdAsync(telefono.Duenio);
            if (persona != null)
            {
                telefono.DuenioNavigation = persona;
                try
                {
                    await _repo.UpdateAsync(telefono);
                }
                catch
                {
                    if (!await _repo.ExistsAsync(telefono.Num))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var personas = await _repo.GetAllPersonasAsync();
            ViewData["Duenio"] = new SelectList(personas, "Cc", "Nombre", telefono.Duenio);
            return View("~/Views/TelefonoMvc/Edit.cshtml", telefono);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var telefono = await _repo.GetByIdAsync(id);
            if (telefono == null) return NotFound();

            return View("~/Views/TelefonoMvc/Delete.cshtml", telefono);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
