using System.ComponentModel.DataAnnotations;

namespace Que.Models
{
    public class Option
    {
        [Key]
        public int OptionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        // Relation til Question
        public int QuestionId { get; set; }
        public virtual Question? Question { get; set; }
    }
}
