using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class IconsController : Controller
    {

        [ActionName("Remix")]
        public IActionResult Remix()
        {
            return View();
        }

        [ActionName("Boxicons")]
        public IActionResult Boxicons()
        {
            return View();
        }

        [ActionName("MaterialDesign")]
        public IActionResult MaterialDesign()
        {
            return View();
        }

        [ActionName("LineAwesome")]
        public IActionResult LineAwesome()
        {
            return View();
        }

        [ActionName("Feather")]
        public IActionResult Feather()
        {
            return View();
        }

        [ActionName("CryptoSVG")]
        public IActionResult CryptoSVG()
        {
            return View();
        }
    }
}
