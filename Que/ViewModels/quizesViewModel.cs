using Que.Models;

namespace Que.ViewModels
{
    public class QuizesViewModel
    {
        public IEnumerable<Quiz> Quizes;
        public string? CurrentViewName;

        public QuizesViewModel(IEnumerable<Quiz> quizes, string? currentViewName)
        {
            Quizes = quizes;
            CurrentViewName = currentViewName;
        }
    }
}