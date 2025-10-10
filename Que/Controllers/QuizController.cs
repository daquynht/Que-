using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Que.Models;

namespace Que.Controllers;

public class QuizController : Controller
{
    public IActionResult Table()
    {
        var Quizes = new List<Quiz>();
        var Quiz1 = new Quiz();
        Quiz1.QuizId = 1;
        Quiz1.Name = "Quiz 1";

        var Quiz2 = new Quiz
        {
            QuizId = 2,
            Name = "Quiz 2",
        };

        Quizes.Add(Quiz1);
        Quizes.Add(Quiz2);

        ViewBag.CurrentViewName = "List of Quizes";
        return View(Quizes);
    }
}