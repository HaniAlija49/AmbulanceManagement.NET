using AmbulanceManagement.Utility;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AmbulanceManagement.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
		public int Number { get; set; }
		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }
		public Gender Gender { get; set; }
		[AllowNull]
		public string Education {  get; set; }
		[AllowNull]
		public string Type { get; set; }
		[AllowNull]
		public string Biography { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }

		public virtual ICollection<Report> Reports { get; set; }
	}
}
