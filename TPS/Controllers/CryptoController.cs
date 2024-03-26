using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class CryptoController : Controller
    {

        [ActionName("Transactions")]
        public IActionResult Transactions()
        {
            return View();
        }

        [ActionName("BuySell")]
        public IActionResult BuySell()
        {
            return View();
        }

        [ActionName("Orders")]
        public IActionResult Orders()
        {
            return View();
        }

        [ActionName("MyWallet")]
        public IActionResult MyWallet()
        {
            return View();
        }

        [ActionName("ICOList")]
        public IActionResult ICOList()
        {
            return View();
        }

        [ActionName("KYCApplication")]
        public IActionResult KYCApplication()
        {
            return View();
        }

    }
}
