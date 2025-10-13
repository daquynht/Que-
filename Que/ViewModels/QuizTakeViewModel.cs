using Microsoft.AspNetCore.Mvc.Rendering;
using Que.Models;

namespace Que.ViewModels
{
    public class QuizTakeViewModel
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = new();
    }
}
