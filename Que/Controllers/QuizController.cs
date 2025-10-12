using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Que.Models;
using Que.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Que.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuizDbContext _quizDbContext;

        public QuizController(QuizDbContext quizDbContext)
        {
            _quizDbContext = quizDbContext;
        }

        // Eksisterende Table
        public IActionResult Table()
        {
            List<Quiz> quizes = _quizDbContext.Quizes.ToList();
            var quizesViewModel = new QuizesViewModel(quizes, "Table");
            return View(quizesViewModel);
        }

        // Ny Take-action
        public IActionResult Take(int id)
        {
            var quiz = _quizDbContext.Quizes
                .Include(q => q.Questions)             // Hent spørsmål
                    .ThenInclude(qs => qs.Options)     // Hent alternativer
                .FirstOrDefault(q => q.QuizId == id);

            if (quiz == null) return NotFound();

            var viewModel = new QuizTakeViewModel
            {
                QuizId = quiz.QuizId,
                QuizName = quiz.Name,
                Questions = quiz.Questions.ToList()
            };

            return View(viewModel); // Viser Take.cshtml
        }
    }
}