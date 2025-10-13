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
            var items = new List<Quiz>
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
            var customers = new List<User>
            {
                new User { Name = "Alice Hansen" },
                new User { Name = "Bob Johansen" },
            };
            context.AddRange(users);
            context.SaveChanges();
        }

        /* if (!context.Orders.Any())
        {
            var orders = new List<Order>
            {
                new Order {OrderDate = DateTime.Today.ToString(), CustomerId = 1,},
                new Order {OrderDate = DateTime.Today.ToString(), CustomerId = 2,},
            };
            context.AddRange(orders);
            context.SaveChanges();
        }

        if (!context.OrderItems.Any())
        {
            var orderItems = new List<OrderItem>
            {
                new OrderItem { ItemId = 1, Quantity = 2, OrderId = 1},
                new OrderItem { ItemId = 2, Quantity = 1, OrderId = 1},
                new OrderItem { ItemId = 3, Quantity = 4, OrderId = 2},
            };
            foreach (var orderItem in orderItems)
            {
                var item = context.Items.Find(orderItem.ItemId);
                orderItem.OrderItemPrice = orderItem.Quantity * item?.Price ?? 0;
            }
            context.AddRange(orderItems);
            context.SaveChanges();
        }

        var ordersToUpdate = context.Orders.Include(o => o.OrderItems);
        foreach (var order in ordersToUpdate)
        {
            order.TotalPrice = order.OrderItems?.Sum(oi => oi.OrderItemPrice) ?? 0;
        }
        context.SaveChanges(); */
    }
}