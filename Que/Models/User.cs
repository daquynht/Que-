using System.ComponentModel.DataAnnotations;

namespace Que.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual List<Quiz>? Quizes { get; set; }
}
