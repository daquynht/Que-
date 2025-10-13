// ViewModels/RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace Que.ViewModels
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, MinLength(2)]
        public string DisplayName { get; set; } = "";

        [Required, MinLength(8)]
        public string Password { get; set; } = "";

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = "";
    }
}
