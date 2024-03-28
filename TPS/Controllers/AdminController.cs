using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPS.Controllers
{
    public class AdminController : Controller
    {
        // GET: /<controller>/

        [ActionName("Index")]
        public IActionResult Index()
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            return View("Dashboard");
        }
    }
}

