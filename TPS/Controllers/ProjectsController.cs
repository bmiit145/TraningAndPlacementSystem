using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class ProjectsController : Controller
    {

        [ActionName("List")]
        public IActionResult List()
        {
            return View();
        }

        [ActionName("Overview")]
        public IActionResult Overview()
        {
            return View();
        }

        [ActionName("CreateProject")]
        public IActionResult CreateProject()
        {
            return View();
        }

    }
}
