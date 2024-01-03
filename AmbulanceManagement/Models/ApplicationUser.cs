using Microsoft.AspNetCore.Identity;

namespace AmbulanceManagement.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
	}
}
