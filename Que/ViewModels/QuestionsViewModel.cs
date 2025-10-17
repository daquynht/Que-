using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Que.Models;

namespace Que.ViewModels
{
    public class QuestionsViewModel
    {
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

        [Required]
            public string Text { get; set; }

            public bool AllowMultipleAnswers { get; set; }

            public List<OptionsViewModel> Options { get; set; }

            /// <summary>
            /// Brukes for binding av valgt(e) alternativ(er)
            /// </summary>
            public List<int> SelectedOptions { get; set; }
    }
}