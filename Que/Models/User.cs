using System.ComponentModel.DataAnnotations;

namespace Que.Models;

// Represents an application user who can create or take quizzes.
public class User
{
    [Key]
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
   
    // Navigation property linking the user to their quizzes
    public virtual List<Quiz>? Quizes { get; set; }
}
