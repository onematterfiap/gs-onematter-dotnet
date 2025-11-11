using System.ComponentModel.DataAnnotations;
using OneMatter.Models;

namespace OneMatter.Models.ViewModels
{
    /// Este ViewModel (DTO) é usado para exibir um candidato ao recrutador
    /// de forma 100% anónima
    public class AnonymousCandidateViewModel
    {
        // Precisamos do ID da *Aplicação* (JobApplication) para o botão "Aprovar"
        public int ApplicationId { get; set; }

        [Display(Name = "Candidato")]
        public string AnonymousId { get; set; } = string.Empty; // Ex: "Candidato #1234"

        [Display(Name = "Habilidades")]
        public string Skills { get; set; } = string.Empty;

        [Display(Name = "Experiência")]
        public string Experiencia { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public ApplicationStatus Status { get; set; }

        [Display(Name = "Pontuação do Teste")]
        public int? SkillScore { get; set; } // Nullable, pois o teste ainda não foi feito
    }
}