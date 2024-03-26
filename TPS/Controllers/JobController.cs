using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class JobController : Controller
    {
        [ActionName("Statistics")]
        public IActionResult Statistics()
        {
            return View();
        }

        [ActionName("Application")]
        public IActionResult Application()
        {
            return View();
        }

        [ActionName("NewJob")]
        public IActionResult NewJob()
        {
            return View();
        }

        [ActionName("CompaniesList")]
        public IActionResult CompaniesList()
        {
            return View();
        }

        [ActionName("JobCategories")]
        public IActionResult JobCategories()
        {
            return View();
        }

        [ActionName("List")]
        public IActionResult List()
        {
            return View();
        }

        [ActionName("Grid")]
        public IActionResult Grid()
        {
            return View();
        }

        [ActionName("Overview")]
        public IActionResult Overview()
        {
            return View();
        }

        [ActionName("ListView")]
        public IActionResult ListView()
        {
            return View();
        }

        [ActionName("GridView")]
        public IActionResult GridView()
        {
            return View();
        }

    }
}
