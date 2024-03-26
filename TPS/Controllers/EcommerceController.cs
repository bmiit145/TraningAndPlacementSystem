using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Velzon.Controllers
{
    public class EcommerceController : Controller
    {

        [ActionName("Products")]
        public IActionResult Products()
        {
            return View();
        }

        [ActionName("ProductDetails")]
        public IActionResult ProductDetails()
        {
            return View();
        }

        [ActionName("CreateProduct")]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [ActionName("Orders")]
        public IActionResult Orders()
        {
            return View();
        }

        [ActionName("OrderDetails")]
        public IActionResult OrderDetails()
        {
            return View();
        }

        [ActionName("Customers")]
        public IActionResult Customers()
        {
            return View();
        }

        [ActionName("ShoppingCart")]
        public IActionResult ShoppingCart()
        {
            return View();
        }

        [ActionName("Checkout")]
        public IActionResult Checkout()
        {
            return View();
        }

        [ActionName("Sellers")]
        public IActionResult Sellers()
        {
            return View();
        }

        [ActionName("SellerDetails")]
        public IActionResult SellerDetails()
        {
            return View();
        }

    }
}
