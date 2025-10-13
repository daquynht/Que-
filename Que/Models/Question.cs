using System.ComponentModel.DataAnnotations;

namespace Que.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;

        // Relation til Quiz
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }

        // Alternativer
        //public List<Option> Options { get; set; } = new();
    }
}
