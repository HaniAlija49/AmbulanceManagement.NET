using AmbulanceManagement.Utility;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AmbulanceManagement.ViewModels
{
    public class RegisterViewModel
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

		public int Number { get; set; }
		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }
		public Gender Gender { get; set; }
		public string Education { get; set; }
		public string Type { get; set; }
        [AllowNull]
		public string ?Biography { get; set; }
        [AllowNull]
        [Display(Name = "Profile Picture")]
        [DataType(DataType.Upload)]
        public IFormFile ProfilePicture { get; set; }
    }
}
