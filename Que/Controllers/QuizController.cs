using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Que.DAL;
using Que.Models;
using Que.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Que.Controllers;

public class QuizController : Controller
{
    private readonly IQuizRepository _quizRepository;
    private readonly ILogger<QuizController> _logger;

    public QuizController(IQuizRepository quizRepository, ILogger<QuizController> logger)
    {
        _quizRepository = quizRepository;
        _logger = logger;
    }

    // =========================
    // GRID VIEW – shows quizzes as cards
    // =========================

    public async Task<IActionResult> Grid()
    {
        var quizes = (await _quizRepository.GetAll()).ToList();
        if (quizes == null)
        {
            _logger.LogError("[QuizController] Quiz list not found while executing _quizRepository.GetAll().ToList()");
            return NotFound("Quiz list not found");
        }
        var viewModel = new QuizesViewModel(quizes, "Grid");
        return View(viewModel);
    }

    // =========================
    // TABLE VIEW – shows quizzes in table format
    // =========================

    public async Task<IActionResult> Table()
    {
        var quizes = (await _quizRepository.GetAll()).ToList();
        if (quizes == null)
        {
            _logger.LogError("[QuizController] Quiz list not found while executing _quizRepository.GetAll().ToList()");
            return NotFound("Quiz list not found");
        }
        var viewModel = new QuizesViewModel(quizes, "Table");
        return View(viewModel);
    }

    // Additional view – same pattern
    public async Task<IActionResult> SeeQuizes()
    {
        var quizes = (await _quizRepository.GetAll()).ToList();
        if (quizes == null)
        {
            _logger.LogError("[QuizController] Quiz list not found while executing _quizRepository.GetAll().ToList()");
            return NotFound("Quiz list not found");
        }
        var viewModel = new QuizesViewModel(quizes, "SeeQuizes");
        return View(viewModel);
    }

    // =========================
    // CREATE (GET) – returns empty form for new quiz
    // =========================

    [HttpGet]
    public IActionResult Create()
    {
        var model = new QuizesViewModel();
        return View(model);
    }


    // =========================
    // CREATE (POST) – builds a new Quiz object from the form data and saves it
    // =========================

    [HttpPost]
    public async Task<IActionResult> Create(QuizesViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var quiz = new Quiz
        {
            Name = model.Quiz.Name,
            Description = model.Quiz.Description,
            Questions = model.Questions.Select(q => new Question
            {
                Text = q.Text,
                AllowMultipleAnswers = q.AllowMultipleAnswers,
                Options = q.Options.Select(o => new Option
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList()
            }).ToList()
        };

        await _quizRepository.Create(quiz);
        return RedirectToAction("Table");
    }

    // =========================
    // UPDATE (GET) – fetches an existing quiz and loads it into a view model
    // =========================

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var quiz = await _quizRepository.GetQuizWithDetailsAsync(id);
        if (quiz == null)
            return NotFound();

        // Map model → view model (including nested relationships)
        var viewModel = new QuizesViewModel
        {
            Quiz = quiz,
            Questions = quiz.Questions.Select(q => new QuestionsViewModel
            {
                QuestionId = q.QuestionId,
                Text = q.Text,
                AllowMultipleAnswers = q.AllowMultipleAnswers,
                Options = q.Options.Select(o => new OptionsViewModel
                {
                    OptionId = o.OptionId,
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList()
            }).ToList()
        };

        return View(viewModel);
    }

    // =========================
    // UPDATE (POST) – saves edited quiz data
    // =========================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(QuizesViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Build updated quiz entity
        var updatedQuiz = new Quiz
        {
            QuizId = model.Quiz.QuizId,
            Name = model.Quiz.Name,
            Description = model.Quiz.Description,
            Category = model.Quiz.Category,
            Difficulty = model.Quiz.Difficulty,
            TimeLimit = model.Quiz.TimeLimit,
            Questions = model.Questions.Select(qvm => new Question
            {
                QuestionId = qvm.QuestionId,
                Text = qvm.Text,
                AllowMultipleAnswers = qvm.AllowMultipleAnswers,
                Options = qvm.Options.Select(ovm => new Option
                {
                    OptionId = ovm.OptionId,
                    Text = ovm.Text,
                    IsCorrect = ovm.IsCorrect
                }).ToList()
            }).ToList()
        };

        // Repository handles database update
        var success = await _quizRepository.UpdateQuizFullAsync(updatedQuiz);

        if (!success)
        {
            ModelState.AddModelError(string.Empty, "En feil oppstod under lagring av endringene.");
            return View(model);
        }
        return RedirectToAction("Table");
    }


    // =========================
    // DELETE (GET) – confirmation page
    // =========================

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

    // =========================
    // DELETE (POST) – deletes the quiz from DB
    // =========================

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _quizRepository.Delete(id);
        return RedirectToAction(nameof(Table));
    }

    // ========================
    // TAKE (GET): viser ett spørsmål av gangen
    // URL: /Quiz/Take/{id}?questionNumber=1
    // ========================

    [HttpGet]
    public async Task<IActionResult> Take(int id, int questionNumber = 1)
    {
        // Fetch quiz and its questions
        var quiz = await _quizRepository.GetQuizById(id);
        var questions = await _quizRepository.GetQuestionsByQuizId(id);

        if (quiz == null || questions == null || !questions.Any())
        {
            return NotFound("Quizen eller dens spørsmål ble ikke funnet.");
        }

        // If questionNumber is out of range, redirect to Result
        if (questionNumber < 1 || questionNumber > questions.Count)
        {
            var key = $"score_{id}";
            int score = 0;
            if (TempData.TryGetValue(key, out var tmp) && tmp is int s)
            {
                score = s;
            }
            TempData.Remove(key);
            return RedirectToAction(nameof(Result), new { id = id, score = score });
        }

        // Build the view model for the current question
        var question = questions[questionNumber - 1];

        var viewModel = new QuizTakeViewModel
        {
            QuizId = quiz.QuizId,
            QuizName = quiz.Name ?? "Ukjent Quiz",
            QuestionNumber = questionNumber,
            TotalQuestions = questions.Count,
            QuestionId = question.QuestionId,
            QuestionText = question.Text,
            AllowMultipleAnswers = question.AllowMultipleAnswers,
            TimeLimit = quiz.TimeLimit, // <- Legg til
            Options = question.Options?.Select(o => new Option
            {
                OptionId = o.OptionId,
                Text = o.Text,
                IsCorrect = o.IsCorrect
            }).ToList() ?? new List<Option>()
        };

        return View(viewModel);
    }

    // ========================
    // TAKE (POST): tar imot svaret, evaluerer og går videre
    // ========================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Take(QuizTakeViewModel model)
    {
        // Get questions from repository
        var questions = await _quizRepository.GetQuestionsByQuizId(model.QuizId);
        if (questions == null || !questions.Any())
        {
            return NotFound();
        }

        // Find current question
        var currentQuestion = questions.FirstOrDefault(q => q.QuestionId == model.QuestionId);
        if (currentQuestion == null)
        {
            ModelState.AddModelError(string.Empty, "Question not found");
            return View(model);
        }

        // If no answer selected → re-display same view with error message
        if (model.SelectedOptionIds == null || !model.SelectedOptionIds.Any())
        {
            ModelState.AddModelError(string.Empty, "Please select at least one answer before continuing.");
            model.Options = currentQuestion.Options?.Select(o => new Option
            {
                OptionId = o.OptionId,
                Text = o.Text,
                IsCorrect = o.IsCorrect
            }).ToList() ?? new List<Option>();
            model.TotalQuestions = questions.Count;
            model.AllowMultipleAnswers = currentQuestion.AllowMultipleAnswers;
            return View(model);
        }

        // Store and update score using TempData (temporary session storage)
        var key = $"score_{model.QuizId}";
        int currentScore = 0;
        if (TempData.TryGetValue(key, out var stored) && stored is int cs)
        {
            currentScore = cs;
        }

        // Check if answer is correct
        if (currentQuestion.AllowMultipleAnswers)
        {
            // Compare sets of correct vs selected options
            var correctOptions = currentQuestion.Options.Where(o => o.IsCorrect).Select(o => o.OptionId).ToHashSet();
            var chosen = model.SelectedOptionIds.ToHashSet();

            if (chosen.SetEquals(correctOptions))
            {
                currentScore += 1; // full score only if all correct
            }
        }
        else
        {
            var chosenOption = currentQuestion.Options.FirstOrDefault(o => o.OptionId == model.SelectedOptionIds.First());
            if (chosenOption != null && chosenOption.IsCorrect)
            {
                currentScore += 1;
            }
        }

        TempData[key] = currentScore; // save updated score

        // Move to next question or finish the quiz
        var nextQuestionNumber = model.QuestionNumber + 1;
        var total = questions.Count;

        if (nextQuestionNumber > total)
        {
            // Quiz finished - calculate final score
            int finalScore = 0;
            if (TempData.TryGetValue(key, out var finalVal) && finalVal is int fs)
            {
                finalScore = fs;
            }
            TempData.Remove(key);

            // Prepare result view model
            var quiz = await _quizRepository.GetQuizById(model.QuizId);
            int totalQuestions = quiz?.Questions?.Count ?? total;

            var resultVm = new ResultViewModel
            {
                QuizId = model.QuizId,
                Score = finalScore,
                TotalQuestions = totalQuestions,
                Percentage = totalQuestions > 0 ? (double)finalScore / totalQuestions * 100.0 : 0.0
            };

            // Redirect to next question
            return View("Result", resultVm);
        }

        return RedirectToAction(nameof(Take), new { id = model.QuizId, questionNumber = nextQuestionNumber });
    }

    // =========================
    // RESULT VIEW – displays final score after quiz is finished
    // =========================
    
    [HttpGet]
    public IActionResult Result(int id, int score)
    {
        // If no score provided, show zero results
        var vm = new ResultViewModel
        {
            QuizId = id,
            Score = score,
            TotalQuestions = 0,
            Percentage = 0
        };
        return View(vm);
    }
}
