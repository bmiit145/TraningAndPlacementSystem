using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class AppsController : Controller
    {
        [ActionName("Calendar")]
        public IActionResult Calendar()
        {
            return View();
        }

        [ActionName("Chat")]
        public IActionResult Chat()
        {
            return View();
        }

        [ActionName("Mailbox")]
        public IActionResult Mailbox()
        {
            return View();
        }

        [ActionName("FileManager")]
        public IActionResult FileManager()
        {
            return View();
        }

        [ActionName("Todo")]
        public IActionResult Todo()
        {
            return View();
        }

        [ActionName("BasicAction")]
        public IActionResult BasicAction()
        {
            return View();
        }

        [ActionName("EcommerceAction")]
        public IActionResult EcommerceAction()
        {
            return View();
        }

        [ActionName("APIKey")]
        public IActionResult APIKey()
        {
            return View();
        }


    }
}
