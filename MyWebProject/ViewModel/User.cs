using System.ComponentModel.DataAnnotations;

namespace MyWebProject.ViewModel
{
    public class User
    {
        [Required(ErrorMessage = "Please enter a new name.")]
        public string NewUsername { get; set; }
    }
}
