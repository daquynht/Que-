namespace Que.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;

        // Relation til Quiz
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        // Alternativer
        public List<Option> Options { get; set; } = new();
    }
}
