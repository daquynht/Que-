using Microsoft.AspNetCore.Mvc;
using Que.Models;
using Microsoft.EntityFrameworkCore;
using Que.DAL;

namespace Que.Controllers;

public class UserController : Controller
{
    private readonly QuizDbContext _quizDbContext;

    public UserController(QuizDbContext quizDbContext)
    {
        _quizDbContext = quizDbContext;
    }

    public async Task<IActionResult> Table()
    {
        List<User> users = await _quizDbContext.Users.ToListAsync();
        return View(users);
    }
}