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
        public string Category { get; set; } = "General";
        public string Difficulty { get; set; } = "Medium";

        [Range(1, 60, ErrorMessage = "Time limit must be between 1 and 60 minutes!")]
        public int TimeLimit { get; set; } = 10;

        public virtual List<Question> Questions { get; set; } = new List<Question>();
    }
}