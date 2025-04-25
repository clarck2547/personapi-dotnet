using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories;

namespace personapi_dotnet.Controllers
{
    public class EstudioMvcController : Controller
    {
        private readonly IEstudioRepository _repo;

        public EstudioMvcController(IEstudioRepository repo)
        {
            _repo = repo;
        }

        // GET: Estudios
        public async Task<IActionResult> Index()
        {
            var estudios = await _repo.GetAllAsync();
            return View("~/Views/EstudioMvc/Index.cshtml", estudios);
        }

        // GET: Estudios/Details/5
        public async Task<IActionResult> Details(int? idProf, int? ccPer)
        {
            if (idProf == null || ccPer == null)
            {
                return NotFound();
            }

            var estudio = await _repo.GetByIdAsync(idProf.Value, ccPer.Value);

            if (estudio == null)
            {
                return NotFound();
            }

            return View("~/Views/EstudioMvc/Details.cshtml", estudio);
        }

        // GET: Estudios/Create
        public async Task<IActionResult> CreateAsync()
        {
            var personas = await _repo.GetAllPersonasAsync();
            var profesiones = await _repo.GetAllProfesionesAsync();
            ViewData["CcPer"] = new SelectList(personas, "Cc", "Nombre");
            ViewData["IdProf"] = new SelectList(profesiones, "Id", "Nom");
            return View();
        }

        // POST: Estudios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            var persona = await _repo.GetPersonaByIdAsync(estudio.CcPer);
            var profesion = await _repo.GetProfesionByIdAsync(estudio.IdProf);

            if (persona != null && profesion != null)
            {
                estudio.CcPerNavigation = persona;
                estudio.IdProfNavigation = profesion;
                await _repo.AddAsync(estudio);
                return RedirectToAction(nameof(Index));
            }
            var personas = await _repo.GetAllPersonasAsync();
            var profesiones = await _repo.GetAllProfesionesAsync();
            ViewData["CcPer"] = new SelectList(personas, "Cc", "Nombre", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(profesiones, "Id", "Nom", estudio.IdProf);
            return View("~/Views/EstudioMvc/Create.cshtml", estudio);
        }

        // GET: Estudios/Edit/5
        public async Task<IActionResult> Edit(int? idProf, int? ccPer)
        {
            if (idProf == null || ccPer == null)
            {
                return NotFound();
            }

            var estudio = await _repo.GetByIdAsync(idProf.Value, ccPer.Value);
            if (estudio == null)
            {
                return NotFound();
            }
            var personas = await _repo.GetAllPersonasAsync();
            var profesiones = await _repo.GetAllProfesionesAsync();
            ViewData["CcPer"] = new SelectList(personas, "Cc", "Nombre", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(profesiones, "Id", "Nom", estudio.IdProf);
            return View("~/Views/EstudioMvc/Edit.cshtml", estudio);
        }

        // POST: Estudios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idProf, int ccPer, [Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer)
            {
                return NotFound();
            }

            var persona = await _repo.GetPersonaByIdAsync(estudio.CcPer);
            var profesion = await _repo.GetProfesionByIdAsync(estudio.IdProf);

            if (persona != null && profesion != null)
            {
                estudio.CcPerNavigation = persona;
                estudio.IdProfNavigation = profesion;
                try
                {
                    await _repo.UpdateAsync(estudio);
                }
                catch
                {
                    if (!await _repo.ExistsAsync(estudio.IdProf, estudio.CcPer))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var personas = await _repo.GetAllPersonasAsync();
            var profesiones = await _repo.GetAllProfesionesAsync();
            ViewData["CcPer"] = new SelectList(personas, "Cc", "Nombre", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(profesiones, "Id", "Nom", estudio.IdProf);
            return View("~/Views/EstudioMvc/Edit.cshtml", estudio);
        }
        
        // GET: Estudios/Delete/5
        public async Task<IActionResult> Delete(int? idProf, int? ccPer)
        {
            if (idProf == null || ccPer == null)
            {
                return NotFound();
            }

            var estudio = await _repo.GetByIdAsync(idProf.Value, ccPer.Value);
            if (estudio == null)
            {
                return NotFound();
            }

            return View("~/Views/EstudioMvc/Delete.cshtml", estudio);
        }

        // POST: Estudios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idProf, int ccPer)
        {
            await _repo.DeleteAsync(idProf, ccPer);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
