using Microsoft.EntityFrameworkCore;
using Que.Models;

namespace Que.DAL;

public class QuizDbContext : DbContext
{
	public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
	{
        // Database.EnsureCreated();  // Remove this line if you use migrations
	}

	public DbSet<Quiz> Quizes { get; set; }
    public DbSet<User> Users { get; set; }
    //public DbSet<Order> Orders { get; set; }
    //public DbSet<OrderItem> OrderItems { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}
