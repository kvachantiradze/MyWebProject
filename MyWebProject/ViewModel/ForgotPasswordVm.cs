using System.ComponentModel.DataAnnotations;

namespace MyWebProject.ViewModel
{
    public class ForgotPasswordVm
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password don't match")]
        public string? ConfirmPassword { get; set; }

    }
}
