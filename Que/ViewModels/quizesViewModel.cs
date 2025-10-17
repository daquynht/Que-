using Que.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Que.ViewModels
{
    public class QuizesViewModel
    {
        public QuizesViewModel()
        {
            Quiz = new Quiz();
                Questions = new List<QuestionsViewModel>
                {
                    new QuestionsViewModel() // Starter med 1 spørsmål
                };
        }

        public Quiz Quiz { get; set; } = new Quiz();
        public List<QuestionsViewModel> Questions { get; set; } = new List<QuestionsViewModel>();


        public IEnumerable<Quiz> Quizes { get; set; } = new List<Quiz>();
        public string? CurrentViewName { get; set; }

        // Nye felt:
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? Difficulty { get; set; }
        public string? QuestionCount { get; set; }

        public QuizesViewModel(IEnumerable<Quiz> quizes, string? currentViewName)
        {
            Quizes = quizes;
            CurrentViewName = currentViewName;
        }

        public List<SelectListItem> DifficultyOptions => new List<SelectListItem>
        {
            new SelectListItem("Easy", "Easy"),
            new SelectListItem("Medium", "Medium"),
            new SelectListItem("Hard", "Hard")
        };

        public List<SelectListItem> CategoryOptions => new List<SelectListItem>
        {
            new SelectListItem("Trivia", "Trivia"),
            new SelectListItem("History", "History"),
            new SelectListItem("Geography", "Geography"),
            new SelectListItem("Math", "Math"),
            new SelectListItem("Science", "Science"),
            new SelectListItem("Sports", "Sports")
        };
    }
}
