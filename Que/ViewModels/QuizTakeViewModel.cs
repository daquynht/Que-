using Microsoft.AspNetCore.Mvc.Rendering;
using Que.Models;

namespace Que.ViewModels
{
    public class QuizTakeViewModel
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; } = string.Empty;

        // (Valgfritt) bruk hvis du vil ha hele spørsmål-lista tilgjengelig
        public List<Question> Questions { get; set; } = new();

        // Nå med feltene som brukes i Controller og View:
        public int QuestionNumber { get; set; }
        public int TotalQuestions { get; set; }           // <-- NY
        public int QuestionId { get; set; }               // <-- NY

        public string QuestionText { get; set; } = string.Empty;
        public int? SelectedOptionId { get; set; }
        public List<Option> Options { get; set; } = new();
    }
}
