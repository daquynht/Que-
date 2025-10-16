using Que.Models;

namespace Que.ViewModels
{
    public class QuizesViewModel
    {
        //public IEnumerable<Quiz> Quizes { get; set; } = new List<Quiz>();
        public List<Quiz>? Quizes { get; set; } = new List<Quiz>();
        public string? CurrentViewName { get; set; }


        public QuizesViewModel(List<Quiz> quizes, string? currentViewName)
        {
            Quizes = quizes;
            CurrentViewName = currentViewName;
        }

        public QuizesViewModel() { }
    }
}
