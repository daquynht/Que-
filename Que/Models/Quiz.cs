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
        public string ImageUrl { get; set; } = string.Empty;

        public string Category { get; set; } = "General";
        public string Difficulty { get; set; } = "Medium";

        public int TimeLimit { get; set; } = 10;

        // Legg til denne for at Include(q => q.Questions) fungerer
        public virtual List<Question>? Questions { get; set; } = new List<Question>();
    }
}