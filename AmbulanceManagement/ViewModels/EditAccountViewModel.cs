using AmbulanceManagement.Utility;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AmbulanceManagement.ViewModels
{
	public class EditAccountViewModel
	{
			public string Id { get; set; }

			[Required(ErrorMessage = "Name is required.")]
			public string Name { get; set; }
			public int Number { get; set; }

			[DataType(DataType.Date)]
			public DateTime DateOfBirth { get; set; }

			public Gender Gender { get; set; }

			public string Education { get; set; }

			public string Type { get; set; }

			[AllowNull]
			public string? Biography { get; set; }
		}

	}
