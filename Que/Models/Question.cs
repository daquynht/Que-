using System.ComponentModel.DataAnnotations;

namespace Que.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;

        // Relation til Quiz
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }

        // Alternativer
        public virtual List<Option> Options { get; set; } = new();
    }
}
