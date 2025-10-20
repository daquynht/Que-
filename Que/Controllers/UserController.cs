using Microsoft.AspNetCore.Mvc;
using Que.Models;
using Microsoft.EntityFrameworkCore;
using Que.DAL;

namespace Que.Controllers;

// Handles user-related pages and data.
// Currently displays all registered users in a table view.
public class UserController : Controller
{
    private readonly QuizDbContext _quizDbContext;

    // Dependency injection of the database context (EF Core)
    public UserController(QuizDbContext quizDbContext)
    {
        _quizDbContext = quizDbContext;
    }

    // =========================    
    // TABLE – Displays all users from the database
    // URL: /User/Table
    // =========================

    public async Task<IActionResult> Table()
    {
        // Asynchronously retrieve all users from the database
        List<User> users = await _quizDbContext.Users.ToListAsync();
        // Pass the user list to the corresponding view
        return View(users);
    }
}