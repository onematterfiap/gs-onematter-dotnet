using Microsoft.AspNetCore.Mvc;
using OneMatter.Data;
using OneMatter.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OneMatter.Models; // <-- ADICIONA ESTE 'USING' para ApplicationStatus

namespace OneMatter.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- 1. LISTAR CANDIDATOS ANÓNIMOS (GET) ---
        // GET: /Applications/Index/5 (onde 5 é o ID da Vaga)
        public async Task<IActionResult> Index(int? id) // 'id' aqui é o JobId
        {
            if (id == null)
            {
                return NotFound("ID da vaga não fornecido.");
            }

            var vaga = await _context.Jobs.FindAsync(id);
            if (vaga == null)
            {
                return NotFound("Vaga não encontrada.");
            }

            ViewData["VagaTitulo"] = vaga.Title;
            ViewData["VagaId"] = vaga.Id;

            var candidatosAnonimos = await _context.JobApplications
                .Include(ja => ja.Candidate)
                .Where(ja => ja.JobId == id)
                .Select(ja => new AnonymousCandidateViewModel
                {
                    ApplicationId = ja.Id,
                    Skills = ja.Candidate.Skills,
                    Experiencia = ja.Candidate.Experiencia,
                    Status = ja.Status,
                    SkillScore = ja.SkillScore,
                    AnonymousId = "Candidato #" + ja.CandidateId

                }).ToListAsync();

            return View(candidatosAnonimos);
        }

        // --- 2. APROVAR CANDIDATO (POST) ---
        // POST: /Applications/Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int applicationId)
        {
            if (applicationId <= 0)
            {
                return NotFound("ID da aplicação inválido.");
            }

            // 1. Encontrar a aplicação (JobApplication) no banco
            var aplicacao = await _context.JobApplications.FindAsync(applicationId);
            if (aplicacao == null)
            {
                return NotFound("Aplicação não encontrada.");
            }

            // 2. Usar o método de domínio para alterar o status
            try
            {
                aplicacao.AprovarParaTeste(); // Muda o Status para Aprovado_Etapa1
                _context.Update(aplicacao);
                await _context.SaveChangesAsync();
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // 3. Redirecionar de volta para a lista de candidatos da VAGA correta
            // É crucial passar o 'id' (que é o JobId) de volta para a ação Index.
            return RedirectToAction("Index", new { id = aplicacao.JobId });
        }
    }
}