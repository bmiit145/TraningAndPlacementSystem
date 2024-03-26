using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class MapsController : Controller
    {

        [ActionName("Google")]
        public IActionResult Google()
        {
            return View();
        }

        [ActionName("Vector")]
        public IActionResult Vector()
        {
            return View();
        }

        [ActionName("Leaflet")]
        public IActionResult Leaflet()
        {
            return View();
        }

    }
}
