using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class CRMController : Controller
    {

        [ActionName("Contacts")]
        public IActionResult Contacts()
        {
            return View();
        }

        [ActionName("Companies")]
        public IActionResult Companies()
        {
            return View();
        }

        [ActionName("Deals")]
        public IActionResult Deals()
        {
            return View();
        }

        [ActionName("Leads")]
        public IActionResult Leads()
        {
            return View();
        }

    }
}
