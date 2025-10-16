using Microsoft.AspNetCore.Mvc;
using Que.DAL;
using Que.Models;
using Que.ViewModels;

namespace Que.Controllers
{
    public class DashboardController : Controller
    {
        private readonly QuizDbContext _context;

        public DashboardController(QuizDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string selectedCategory)
        {
            var quizes = _context.Quizes.ToList(); // hent alle quizer fra DB

            if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "all")
            {
                quizes = quizes.Where(q => q.Category == selectedCategory).ToList();
            }

            var model = new DashboardViewModel
            {
                Quizes = quizes,
                SelectedCategory = selectedCategory ?? "all"
            };

            return View(model);
        }

        public IActionResult Table() 
        {
            var viewModel = new QuizesViewModel(_context.Quizes.ToList(), "Table");
            return View(viewModel);
        }

        public IActionResult Grid()
        {   
            var quizes = _context.Quizes.ToList();
            return View(quizes);
        }
        
    }
}