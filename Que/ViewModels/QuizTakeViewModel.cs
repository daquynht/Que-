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
        public int TotalQuestions { get; set; }
        public int QuestionId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public List<int> SelectedOptionIds { get; set; } = new();
        public List<Option> Options { get; set; } = new();

        public bool AllowMultipleAnswers { get; set; }

        public int TimeLimitInSeconds => TimeLimit * 60;
        public int TimeLimit { get; set; }
    }
}
