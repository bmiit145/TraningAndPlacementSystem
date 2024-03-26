using Microsoft.AspNetCore.Mvc;

namespace Velzon.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NFTLanding()
        {
            return View();
        }
        public IActionResult Job()
        {
            return View();
        }
    }
}
