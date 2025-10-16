using Microsoft.AspNetCore.Mvc;
using Que.Models;
using Que.DAL;

namespace Que.Controllers
{
    public class QuestionController : Controller
    {
        private readonly QuizDbContext _db;

        public QuestionController(QuizDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Create(int quizId)
        {
            var question = new Question { QuizId = quizId, Options = new List<Option> {
                new Option(), new Option(), new Option(), new Option()
            }};
            ViewBag.QuizId = quizId;
            return View(question);
        }

        [HttpPost]
        public IActionResult Create(Question question)
        {
            if (ModelState.IsValid)
            {
                // Remove empty options
                question.Options = question.Options?.Where(o => !string.IsNullOrWhiteSpace(o.Text)).ToList() ?? new List<Option>();
                _db.Questions.Add(question);
                _db.SaveChanges();
                return RedirectToAction("Create", new { quizId = question.QuizId });
            }
            ViewBag.QuizId = question.QuizId;
            return View(question);
        }
    }
}
