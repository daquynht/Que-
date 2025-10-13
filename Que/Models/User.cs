namespace Que.Models;

public class User
{
    public int QuizId { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual List<Quiz>? Quizes { get; set; }
}
