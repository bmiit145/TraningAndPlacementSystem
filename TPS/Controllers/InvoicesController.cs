using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class InvoicesController : Controller
    {

        [ActionName("ListView")]
        public IActionResult ListView()
        {
            return View();
        }

        [ActionName("Details")]
        public IActionResult Details()
        {
            return View();
        }

        [ActionName("CreateInvoice")]
        public IActionResult CreateInvoice()
        {
            return View();
        }

    }
}
