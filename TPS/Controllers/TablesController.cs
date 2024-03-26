using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class TablesController : Controller
    {
        [ActionName("BasicTables")]
        public IActionResult BasicTables()
        {
            return View();
        }

        [ActionName("GridJs")]
        public IActionResult GridJs()
        {
            return View();
        }

        [ActionName("ListJs")]
        public IActionResult ListJs()
        {
            return View();
        }
        [ActionName("Datatables")]
        public IActionResult Datatables()
        {
            return View();
        }
    }
}
