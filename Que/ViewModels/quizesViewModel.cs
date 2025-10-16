using Que.Models;

namespace Que.ViewModels
{
    public class QuizesViewModel
    {
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

        public QuizesViewModel() { }
    }
}
