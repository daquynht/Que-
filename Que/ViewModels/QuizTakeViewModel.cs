using Microsoft.AspNetCore.Mvc.Rendering;
using Que.Models;

namespace Que.ViewModels
{
    public class QuizTakeViewModel
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = new();
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int? SelectedOptionId { get; set; }
        public List<Option> Options { get; set; } = new();

    }
}
