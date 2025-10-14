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
        var quizes = await _quizRepository.GetAll();
        var viewModel = new QuizesViewModel(quizes, "Grid");
        return View(viewModel);
    }

    public async Task<IActionResult> Table()
    {
        var quizes = await _quizRepository.GetAll();
        var quizesViewModel = new QuizesViewModel(quizes, "Table");
        return View(quizesViewModel);
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
            return RedirectToAction(nameof(Table));
        }

        return View(quiz);
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