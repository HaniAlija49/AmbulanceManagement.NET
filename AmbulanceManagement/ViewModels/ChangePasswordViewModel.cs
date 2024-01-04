using System.ComponentModel.DataAnnotations;

namespace AmbulanceManagement.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
