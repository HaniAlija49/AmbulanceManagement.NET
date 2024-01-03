using System.ComponentModel.DataAnnotations;

namespace AmbulanceManagement.ViewModels
{
    public class Register
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100,ErrorMessage = " the {0} must be at least {2} character long.", MinimumLength = 6)]

        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set;}

        [Required]
        [Display(Name ="RoleName Name")]

        public string RoleName { get; set; }
    }
}
