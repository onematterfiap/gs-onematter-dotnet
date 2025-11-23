using Microsoft.AspNetCore.Mvc;
using OneMatter.Models;
using OneMatter.Models.ViewModels;
using OneMatter.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OneMatter.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly JobService _service;
        public JobsController(JobService service)
        {
            _service = service;
        }

        // --- 1. LISTAR (INDEX) ---
        // GET: /Jobs/
        public async Task<IActionResult> Index()
        {
            var vagas = await _service.GetAllJobsAsync();
            return View(vagas);
        }

        // --- 2. CRIAR (GET) ---
        // GET: /Jobs/Create
        public IActionResult Create()
        {
            var viewModel = new CreateJobViewModel();
            return View(viewModel);
        }

        // --- 3. CRIAR (POST) ---
        // POST: /Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateJobAsync(
                    viewModel.Title,
                    viewModel.Description,
                    viewModel.Location
                );
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // --- 4. EDITAR (GET) ---
        // GET: /Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vaga = await _service.GetJobByIdAsync(id.Value);
            if (vaga == null) return NotFound();

            var viewModel = new CreateJobViewModel
            {
                Title = vaga.Title,
                Description = vaga.Description,
                Location = vaga.Location
            };

            return View(viewModel);
        }

        // --- 5. EDITAR (POST) ---
        // POST: /Jobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateJobViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateJobAsync(
                        id,
                        viewModel.Title,
                        viewModel.Description,
                        viewModel.Location
                    );
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (InvalidOperationException ex) // Captura regras de negócio (ex: vaga fechada)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(viewModel);
        }

        // --- 6. DETALHES (GET) ---
        // GET: /Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var vaga = await _service.GetJobByIdAsync(id.Value);
            if (vaga == null) return NotFound();

            return View(vaga);
        }

        // --- 7. EXCLUIR (GET) ---
        // GET: /Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vaga = await _service.GetJobByIdAsync(id.Value);
            if (vaga == null) return NotFound();

            return View(vaga);
        }

        // --- 8. EXCLUIR (POST) ---
        // POST: /Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteJobAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}