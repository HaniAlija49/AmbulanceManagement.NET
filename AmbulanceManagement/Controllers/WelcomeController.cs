using Microsoft.AspNetCore.Mvc;

namespace AmbulanceManagement.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
