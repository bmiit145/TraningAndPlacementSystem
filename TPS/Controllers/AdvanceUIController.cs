using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class AdvanceUIController : Controller
    {

        [ActionName("SweetAlerts")]
        public IActionResult SweetAlerts()
        {
            return View();
        }

        [ActionName("NestableList")]
        public IActionResult NestableList()
        {
            return View();
        }

        [ActionName("Scrollbar")]
        public IActionResult Scrollbar()
        {
            return View();
        }

        [ActionName("Animation")]
        public IActionResult Animation()
        {
            return View();
        }

        [ActionName("Tour")]
        public IActionResult Tour()
        {
            return View();
        }

        [ActionName("SwiperSlider")]
        public IActionResult SwiperSlider()
        {
            return View();
        }

        [ActionName("Ratings")]
        public IActionResult Ratings()
        {
            return View();
        }

        [ActionName("Highlight")]
        public IActionResult Highlight()
        {
            return View();
        }

        [ActionName("ScrollSpy")]
        public IActionResult ScrollSpy()
        {
            return View();
        }

    }
}
