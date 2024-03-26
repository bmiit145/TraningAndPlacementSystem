using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class BaseUIController : Controller
    {

        [ActionName("Alerts")]
        public IActionResult Alerts()
        {
            return View();
        }

        [ActionName("Badges")]
        public IActionResult Badges()
        {
            return View();
        }
        [ActionName("Links")]
        public IActionResult Links()
        {
            return View();
        }

        [ActionName("Buttons")]
        public IActionResult Buttons()
        {
            return View();
        }

        [ActionName("Colors")]
        public IActionResult Colors()
        {
            return View();
        }

        [ActionName("Cards")]
        public IActionResult Cards()
        {
            return View();
        }

        [ActionName("Carousel")]
        public IActionResult Carousel()
        {
            return View();
        }

        [ActionName("Dropdowns")]
        public IActionResult Dropdowns()
        {
            return View();
        }

        [ActionName("Grid")]
        public IActionResult Grid()
        {
            return View();
        }

        [ActionName("Images")]
        public IActionResult Images()
        {
            return View();
        }

        [ActionName("Tabs")]
        public IActionResult Tabs()
        {
            return View();
        }

        [ActionName("AccordionCollapse")]
        public IActionResult AccordionCollapse()
        {
            return View();
        }

        [ActionName("Modals")]
        public IActionResult Modals()
        {
            return View();
        }

        [ActionName("Offcanvas")]
        public IActionResult Offcanvas()
        {
            return View();
        }

        [ActionName("Placeholders")]
        public IActionResult Placeholders()
        {
            return View();
        }

        [ActionName("Progress")]
        public IActionResult Progress()
        {
            return View();
        }

        [ActionName("Notifications")]
        public IActionResult Notifications()
        {
            return View();
        }

        [ActionName("Mediaobject")]
        public IActionResult Mediaobject()
        {
            return View();
        }

        [ActionName("EmbedVideo")]
        public IActionResult EmbedVideo()
        {
            return View();
        }

        [ActionName("Typography")]
        public IActionResult Typography()
        {
            return View();
        }

        [ActionName("Lists")]
        public IActionResult Lists()
        {
            return View();
        }

        [ActionName("General")]
        public IActionResult General()
        {
            return View();
        }

        [ActionName("Ribbons")]
        public IActionResult Ribbons()
        {
            return View();
        }

        [ActionName("Utilities")]
        public IActionResult Utilities()
        {
            return View();
        }

    }
}
