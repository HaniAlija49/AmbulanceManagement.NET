using System.ComponentModel.DataAnnotations;

namespace AmbulanceManagement.Models
{
	public class Patient
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(50)]
		public string Name { get; set; }
		[Required,StringLength(50)]
		public string LastName { get; set; }
		[Required]
		[EmailAddress] 
		public string EmailAddress { get; set;}
		[Required]
		[StringLength(9)]
		public string PhoneNumber { get; set; }
		[Required]
		public int Age { get; set; }
		[Required]
		[StringLength (100)]
		public string Adress { get; set; }

        public virtual ICollection<Appointment> ?Appointments { get; set; }
    }
}
