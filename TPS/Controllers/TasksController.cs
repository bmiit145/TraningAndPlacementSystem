using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class TasksController : Controller
    {

        [ActionName("KanbanBoard")]
        public IActionResult KanbanBoard()
        {
            return View();
        }

        [ActionName("ListView")]
        public IActionResult ListView()
        {
            return View();
        }

        [ActionName("TaskDetails")]
        public IActionResult TaskDetails()
        {
            return View();
        }

    }
}
