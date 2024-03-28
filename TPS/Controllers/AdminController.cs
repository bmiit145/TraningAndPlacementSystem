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
            return View("Dashboard");
        }
    }
}

