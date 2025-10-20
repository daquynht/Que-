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

    public async Task<IActionResult> Grid()
    {
        var quizes = (await _quizRepository.GetAll()).ToList();
        var viewModel = new QuizesViewModel(quizes, "Grid");
        return View(viewModel);
    }

    public async Task<IActionResult> Table()
    {
        _logger.LogInformation("This is an information message.");
        _logger.LogWarning("This is a warning message.");
        _logger.LogError("This is an error message.");

        var quizes = (await _quizRepository.GetAll()).ToList();
        var viewModel = new QuizesViewModel(quizes, "Table");
        return View(viewModel);
    }

    public async Task<IActionResult> SeeQuizes()
    {
        var quizes = (await _quizRepository.GetAll()).ToList();
        var viewModel = new QuizesViewModel(quizes, "SeeQuizes");
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new QuizesViewModel();
        return View(model);
    }

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

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var quiz = await _quizRepository.GetQuizWithDetailsAsync(id);
        if (quiz == null)
            return NotFound();

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(QuizesViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

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

        var success = await _quizRepository.UpdateQuizFullAsync(updatedQuiz);

        if (!success)
        {
            ModelState.AddModelError(string.Empty, "En feil oppstod under lagring av endringene.");
            return View(model);
        }
        return RedirectToAction("Table");
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

    // ========================
    // TAKE (GET): viser ett spørsmål av gangen
    // URL: /Quiz/Take/{id}?questionNumber=1
    // ========================
    [HttpGet]
    public async Task<IActionResult> Take(int id, int questionNumber = 1)
    {
        // Hent quiz + spørsmål fra repo
        var quiz = await _quizRepository.GetQuizById(id);
        var questions = await _quizRepository.GetQuestionsByQuizId(id);

        if (quiz == null || questions == null || !questions.Any())
        {
            return NotFound("Quizen eller dens spørsmål ble ikke funnet.");
        }

        // Hvis questionNumber utenfor range -> vis resultat
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
        // Hent spørsmål fra repo
        var questions = await _quizRepository.GetQuestionsByQuizId(model.QuizId);
        if (questions == null || !questions.Any())
        {
            return NotFound();
        }

        // Finn gjeldende spørsmål
        var currentQuestion = questions.FirstOrDefault(q => q.QuestionId == model.QuestionId);
        if (currentQuestion == null)
        {
            ModelState.AddModelError(string.Empty, "Spørsmålet finnes ikke.");
            return View(model);
        }

        // Hvis ingen svar valgt -> vis samme view med feilmelding (repoppuler options)
        // Hvis ingen svar valgt -> vis samme view med feilmelding
        if (model.SelectedOptionIds == null || !model.SelectedOptionIds.Any())
        {
            ModelState.AddModelError(string.Empty, "Vennligst velg minst ett svar før du fortsetter.");
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

        // Les/oppdater score
        var key = $"score_{model.QuizId}";
        int currentScore = 0;
        if (TempData.TryGetValue(key, out var stored) && stored is int cs)
        {
            currentScore = cs;
        }

        // Evaluer riktig/flere svar
        if (currentQuestion.AllowMultipleAnswers)
        {
            var correctOptions = currentQuestion.Options.Where(o => o.IsCorrect).Select(o => o.OptionId).ToHashSet();
            var chosen = model.SelectedOptionIds.ToHashSet();

            if (chosen.SetEquals(correctOptions))
            {
                currentScore += 1; // full score hvis alle riktige (og ingen feil)
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

        TempData[key] = currentScore; // lagre score

        // Neste spørsmål eller resultat
        var nextQuestionNumber = model.QuestionNumber + 1;
        var total = questions.Count;

        if (nextQuestionNumber > total)
        {
            // Avslutt quiz: hent siste score og rydd TempData
            int finalScore = 0;
            if (TempData.TryGetValue(key, out var finalVal) && finalVal is int fs)
            {
                finalScore = fs;
            }
            TempData.Remove(key);

            // Hent quiz for å få metadata (f.eks total antall spørsmål)
            var quiz = await _quizRepository.GetQuizById(model.QuizId);
            int totalQuestions = quiz?.Questions?.Count ?? total;

            var resultVm = new ResultViewModel
            {
                QuizId = model.QuizId,
                Score = finalScore,
                TotalQuestions = totalQuestions,
                Percentage = totalQuestions > 0 ? (double)finalScore / totalQuestions * 100.0 : 0.0
            };

            // Returner resultat view med viewmodel
            return View("Result", resultVm);
        }

        return RedirectToAction(nameof(Take), new { id = model.QuizId, questionNumber = nextQuestionNumber });
    }

    // RESULT: viser resultatet
    [HttpGet]
    public IActionResult Result(int id, int score)
    {
        // Hvis noen ruter kaller denne direkte med id/score, bygg en enkel viewmodel
        var vm = new ResultViewModel
        {
            QuizId = id,
            Score = score,
            TotalQuestions = 0,
            Percentage = 0
        };
        return View(vm);
    }

    // Du kan la AddQuestion ligge kommentert eller fjerne den — behold slik du foretrekker.
}
