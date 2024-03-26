using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class LayoutsController : Controller
    {

        [ActionName("Horizontal")]
        public IActionResult Horizontal()
        {
            return View();
        }

        [ActionName("Detached")]
        public IActionResult Detached()
        {
            return View();
        }

        [ActionName("TwoColumn")]
        public IActionResult TwoColumn()
        {
            return View();
        }

        [ActionName("Hovered")]
        public IActionResult Hovered()
        {
            return View();
        }

    }
}
