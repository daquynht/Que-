using Microsoft.AspNetCore.Mvc;
using Que.DAL;
using Que.Models;
using Que.ViewModels;

namespace Que.Controllers;

public class QuizController : Controller
{
    private readonly IQuizRepository _quizRepository;

    public QuizController(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<IActionResult> Grid()
    {
        var quizes = (await _quizRepository.GetAll()).ToList();
        var viewModel = new QuizesViewModel(quizes, "Grid");
        return View(viewModel);
    }

    public async Task<IActionResult> Table()
    {
        var quizes = (await _quizRepository.GetAll()).ToList();
        var viewModel = new QuizesViewModel(quizes, "Table");
        return View(viewModel);
    }
    /* public async Task<IActionResult> Table(string searchTerm, string category, string difficulty, string questionCount)
    {
        var quizes = await _quizRepository.GetAll();

        // 🔍 Filterlogikk
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            quizes = quizes.Where(q =>
                (q.Name != null && q.Name.ToLower().Contains(searchTerm)) ||
                (q.Description != null && q.Description.ToLower().Contains(searchTerm)) ||
                (q.Category != null && q.Category.ToLower().Contains(searchTerm))
            ).ToList();
        }

        if (!string.IsNullOrEmpty(category) && category != "all")
            quizes = quizes.Where(q => q.Category == category).ToList();

        if (!string.IsNullOrEmpty(difficulty) && difficulty != "all")
            quizes = quizes.Where(q => q.Difficulty == difficulty).ToList();

        if (!string.IsNullOrEmpty(questionCount) && questionCount != "all")
        {
            var ranges = new Dictionary<string, (int min, int max)>
            {
                { "1-5", (1, 5) },
                { "6-10", (6, 10) },
                { "11-15", (11, 15) },
                { "16-20", (16, 20) },
                { "20+", (21, int.MaxValue) }
            };

            if (ranges.TryGetValue(questionCount, out var range))
            {
                quizes = quizes.Where(q => q.Questions.Count >= range.min && q.Questions.Count <= range.max).ToList();
            }
        }

        // ✅ Send tilbake viewmodel
        var quizesViewModel = new QuizesViewModel(quizes, "Table")
        {
            SearchTerm = searchTerm,
            Category = category,
            Difficulty = difficulty,
            QuestionCount = questionCount
        };

        return View(quizesViewModel);
    } */

    public async Task<IActionResult> SeeQuizes()
    {
        var quizes = (await _quizRepository.GetAll()).ToList();
        var viewModel = new QuizesViewModel(quizes, "SeeQuizes");
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Quiz quiz)
    {
        if (ModelState.IsValid)
        {
            await _quizRepository.Create(quiz);
            // Redirect to CreateQuestions page for this quiz
            return RedirectToAction("CreateQuestions", new { quizId = quiz.QuizId });
        }

        return View(quiz);
    }

    [HttpGet]
    public IActionResult CreateQuestions(int quizId)
    {
        ViewBag.QuizId = quizId;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var quiz = await _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            return NotFound();
        }
        return View(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Quiz quiz)
    {
        if (ModelState.IsValid)
        {
            await _quizRepository.Update(quiz);
            return RedirectToAction(nameof(Table));
        }

        return View(quiz);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var quiz = await _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            return NotFound();
        }
        return View(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _quizRepository.Delete(id);
        return RedirectToAction(nameof(Table));
    }
}