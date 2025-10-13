using Microsoft.EntityFrameworkCore;
using Que.Models;

namespace Que.DAL;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        QuizDbContext context = serviceScope.ServiceProvider.GetRequiredService<QuizDbContext>();
        // context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Quizes.Any())
        {
            var quizes = new List<Quiz>
            {
                new Quiz
                {
                    Name = "Pizza",
                    Description = "Delicious Italian dish with a thin crust topped with tomato sauce, cheese, and various toppings."
                },
                new Quiz
                {
                    Name = "Fried Chicken Leg",
                    Description = "Crispy and succulent chicken leg that is deep-fried to perfection, often served as a popular fast food item."
                },
                new Quiz
                {
                    Name = "French Fries",
                    Description = "Crispy, golden-brown potato slices seasoned with salt and often served as a popular side dish or snack."
                }
            };
            context.AddRange(quizes);
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User { Name = "Alice Hansen" },
                new User { Name = "Bob Johansen" },
            };
            context.AddRange(users);
            context.SaveChanges();
        }
        context.SaveChanges();
    }
}