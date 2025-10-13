using System;
using System.ComponentModel.DataAnnotations;
using Que.Models;

namespace Que.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Legg til denne for at Include(q => q.Questions) fungerer
        public virtual List<Question> Questions { get; set; } = new List<Question>();
    }
}