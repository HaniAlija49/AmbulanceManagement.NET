using Microsoft.AspNetCore.Mvc;

namespace AmbulanceManagement.Views
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
