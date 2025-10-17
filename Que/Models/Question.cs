using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Que.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;

        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }

        public virtual List<Option> Options { get; set; } = new List<Option>();

        public bool AllowMultipleAnswers { get; set; } = false;

        [NotMapped]
        public int? SelectedOption { get; set; }
    }
}
