namespace Que.ViewModels
{
    public class ResultViewModel
    {
        public int QuizId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public double Percentage { get; set; }

        // Valgfritt : tekstmelding
        public string Message => $"You scored {Score} / {TotalQuestions} ({Percentage:F1}%)";
    }
}
