namespace Que.Models
{
    public class Option
    {
        public int OptionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        // Relation til Question
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
