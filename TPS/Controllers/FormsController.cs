using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class FormsController : Controller
    {

        [ActionName("BasicElements")]
        public IActionResult BasicElements()
        {
            return View();
        }

        [ActionName("FormSelect")]
        public IActionResult FormSelect()
        {
            return View();
        }

        [ActionName("CheckboxsRadios")]
        public IActionResult CheckboxsRadios()
        {
            return View();
        }

        [ActionName("Pickers")]
        public IActionResult Pickers()
        {
            return View();
        }

        [ActionName("InputMasks")]
        public IActionResult InputMasks()
        {
            return View();
        }

        [ActionName("Advanced")]
        public IActionResult Advanced()
        {
            return View();
        }

        [ActionName("RangeSlider")]
        public IActionResult RangeSlider()
        {
            return View();
        }

        [ActionName("Validation")]
        public IActionResult Validation()
        {
            return View();
        }

        [ActionName("Wizard")]
        public IActionResult Wizard()
        {
            return View();
        }

        [ActionName("Editors")]
        public IActionResult Editors()
        {
            return View();
        }

        [ActionName("FileUploads")]
        public IActionResult FileUploads()
        {
            return View();
        }

        [ActionName("FormLayouts")]
        public IActionResult FormLayouts()
        {
            return View();
        }

        [ActionName("Select2")]
        public IActionResult Select2()
        {
            return View();
        }
    }
}
