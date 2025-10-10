using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Que.Models;
using Que.ViewModels;

namespace Que.Controllers;

public class QuizController : Controller
{
    public IActionResult Table()
    {
        var quizes = GetQuizes();
        var quizesViewModel = new QuizesViewModel(quizes, "Table");
        return View(quizesViewModel);
    }

    public List<Quiz> GetQuizes()
    {
        var quizes = new List<Quiz>();
        var quiz1 = new Quiz
        {
            QuizId = 1,
            Name = "Quiz test",
            Description = "Test",
        };

        var quiz2 = new Quiz
        {
            QuizId = 2,
            Name = "Quiz test2",
            Description = "Test2",
        };

        quizes.Add(quiz1);
        quizes.Add(quiz2);
        return quizes;
    }
}