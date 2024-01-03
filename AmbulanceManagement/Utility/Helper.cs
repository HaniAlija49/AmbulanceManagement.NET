using Microsoft.AspNetCore.Mvc.Rendering;

namespace AmbulanceManagement.Utility
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Doctor = "Doctor";
		public static string Nurse = "Nurse";

		public static List<SelectListItem> GetRolesForDropDown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Value=Helper.Admin, Text=Helper.Admin},
                new SelectListItem{Value=Helper.Doctor, Text=Helper.Doctor},
				new SelectListItem{Value=Helper.Nurse, Text=Helper.Nurse},
			};     
        }
    }
}
