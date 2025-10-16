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
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
    // Legg til denne metoden i DAL/QuizDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // --- Quiz Seeding ---
    modelBuilder.Entity<Quiz>().HasData(
        new Quiz { QuizId = 1, Name = "General Knowledge Basics", Description = "Test your basic knowledge.", ImageUrl = "/images/default_quiz.jpg" }
    );
    
    // --- Question Seeding ---
    // VIKTIG: Pass på at du bruker nye, unike ID-er her
    modelBuilder.Entity<Question>().HasData(
        new Question { QuestionId = 1, QuizId = 1, Text = "What is the capital of Norway?" },
        new Question { QuestionId = 2, QuizId = 1, Text = "What is the largest planet in our solar system?" }
    );

    // --- Option Seeding (NYTT!) ---
    // Du må koble hvert alternativ til riktig QuestionId (1 eller 2)
    modelBuilder.Entity<Option>().HasData(
        // Alternativer for Spørsmål 1: What is the capital of Norway?
        new Option { OptionId = 1, QuestionId = 1, Text = "Bergen", IsCorrect = false },
        new Option { OptionId = 2, QuestionId = 1, Text = "Oslo", IsCorrect = true }, // Riktig svar
        new Option { OptionId = 3, QuestionId = 1, Text = "Trondheim", IsCorrect = false },
        new Option { OptionId = 4, QuestionId = 1, Text = "Stavanger", IsCorrect = false },

        // Alternativer for Spørsmål 2: What is the largest planet?
        new Option { OptionId = 5, QuestionId = 2, Text = "Saturn", IsCorrect = false },
        new Option { OptionId = 6, QuestionId = 2, Text = "Jupiter", IsCorrect = true }, // Riktig svar
        new Option { OptionId = 7, QuestionId = 2, Text = "Mars", IsCorrect = false },
        new Option { OptionId = 8, QuestionId = 2, Text = "Jorden", IsCorrect = false }
    );
    
    // Basismetoden kalles til slutt
    base.OnModelCreating(modelBuilder);
}
}
