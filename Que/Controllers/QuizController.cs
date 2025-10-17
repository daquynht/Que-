using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        var quiz = new Quiz
        {
            Questions = new List<Question>
            {
                new Question
                {
                    Options = new List<Option> { new Option(), new Option(), new Option(), new Option() }
                },
                new Question
                {
                    Options = new List<Option> { new Option(), new Option(), new Option(), new Option() }
                }
            }
        };

        // Viktig: sørg for at QuestionId = 0 og OptionId = 0
        foreach (var question in quiz.Questions)
        {
            question.QuestionId = 0;
            foreach (var option in question.Options)
            {
                option.OptionId = 0;
            }
        }

        return View(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Quiz quiz)
    {
        if (!ModelState.IsValid) return View(quiz);

        // Nullstill ID-er for sikkerhet
        foreach (var question in quiz.Questions)
        {
            question.QuestionId = 0;
            question.QuizId = 0;
            foreach (var option in question.Options)
            {
                option.OptionId = 0;
                option.QuestionId = 0;
            }
        }

        await _quizRepository.Create(quiz); // Her lagres Quiz med spørsmål og alternativer

        return RedirectToAction(nameof(Table));
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


    [HttpGet]
    public async Task<IActionResult> Take(int id) // 'id' er QuizId
    {
        // 1. Henter quizen og dens spørsmål
        var quiz = await _quizRepository.GetQuizById(id);
        
        // *DEN KRITISKE LINJEN:* Kaller metoden du implementerte
        var questions = await _quizRepository.GetQuestionsByQuizId(id); 

        if (quiz == null || questions == null || !questions.Any())
        {
            return NotFound("Quizen eller dens spørsmål ble ikke funnet.");
        }

        // 2. Velger det første spørsmålet (for MVP)
        var firstQuestion = questions.First();

        // 3. Setter opp ViewModel for visning
        var viewModel = new QuizTakeViewModel
        {
            QuizId = quiz.QuizId,
            QuizName = quiz.Name ?? "Ukjent Quiz",
            QuestionNumber = 1, // Start med spørsmål 1
            QuestionText = firstQuestion.Text,
            
            // Hardkoder alternativer for MVP (som vi avtalte)
            // I et fullstendig prosjekt ville disse kommet fra en Option-tabell
            Options = new List<Option>
            {
                new Option { OptionId = 1, Text = "Alternativ A" },
                new Option { OptionId = 2, Text = "Alternativ B" },
                new Option { OptionId = 3, Text = "Alternativ C" },
                new Option { OptionId = 4, Text = "Alternativ D" }
            },
            SelectedOptionId = null // Initialiseres uten svar
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Take(QuizTakeViewModel model)
    {
        // Denne metoden er foreløpig enkel, men vil senere håndtere
        // 1. Validering av svaret (mot CorrectAnswer)
        // 2. Oppdatering av score
        // 3. Viderekobling til neste spørsmål eller resultat-side
        
        if (model.SelectedOptionId.HasValue)
        {
            // For MVP-demonstrasjon, bare viderekoble til samme side
            // I et komplett prosjekt, vil du lagre svaret og gå til neste spørsmål/resultat
            return RedirectToAction(nameof(Take), new { id = model.QuizId, questionNumber = model.QuestionNumber + 1 });
        }
        
        // Hvis ingen alternativ er valgt
        ModelState.AddModelError(string.Empty, "Vennligst velg et svar før du fortsetter.");
        return View(model); 
    }

    [HttpPost]
    public IActionResult AddQuestion(Quiz quiz, List<Question> questions)
    {
        // Først beholder eksisterende spørsmål fra skjemaet
        if (questions != null)
        {
            quiz.Questions = questions;
        }

        // Legg til nytt spørsmål med 4 tomme alternativer
        quiz.Questions.Add(new Question
        {
            Options = new List<Option>
            {
                new Option(), new Option(), new Option(), new Option()
            }
        });

        return View("Create", quiz);
    }

}