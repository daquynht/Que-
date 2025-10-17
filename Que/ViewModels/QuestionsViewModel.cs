using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Que.Models;

namespace Que.ViewModels
{
    public class QuestionsViewModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool AllowMultipleAnswers { get; set; }

        public List<OptionsViewModel> Options { get; set; } = new List<OptionsViewModel>();
        public List<int> SelectedOptions { get; set; }
        public QuestionsViewModel()
        {
            Options = new List<OptionsViewModel>
            {
                new OptionsViewModel(),
                new OptionsViewModel(),
                new OptionsViewModel(),
                new OptionsViewModel()
            };
            SelectedOptions = new List<int>();
        }
    }
}