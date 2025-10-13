using Microsoft.AspNetCore.Mvc;
using Que.DAL;
using Que.Models;

namespace Que.Controllers
{
    public class DashboardController : Controller
    {
        private readonly QuizDbContext _context;

        public DashboardController(QuizDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var quizzes = _context.Quizes.ToList(); // henter alle quizene fra databasen
            return View(quizzes);                   // sender dem til viewet
        }
    }
}
