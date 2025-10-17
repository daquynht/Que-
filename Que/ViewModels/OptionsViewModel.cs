using System.ComponentModel.DataAnnotations;
using Que.Models;

namespace Que.ViewModels
{
    public class OptionsViewModel
        {
            [Required]
            public string Text { get; set; }
        }
}