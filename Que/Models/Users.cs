namespace Que.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } = "";
        public string DisplayName { get; set; } = "";
        publiic string PasswordHash { get; set; } = "";
    }
}