using Microsoft.EntityFrameworkCore;

namespace Que.Models;

public class QuizDbContext : DbContext
{
	public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
	{
        Database.EnsureCreated();
	}

	public DbSet<Quiz> Quizes { get; set; }
}