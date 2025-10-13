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
            var quizes = _context.Quizes.ToList(); // henter alle quizene fra databasen
            return View(quizes);                   // sender dem til viewet
        }
    }
}
