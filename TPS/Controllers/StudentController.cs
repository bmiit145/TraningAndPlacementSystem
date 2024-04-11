using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TPS.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPS.Controllers
{
    public class StudentController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "0")
            {
                return RedirectToAction("SignIn", "Authentication");
            }


            // get all the videos
            ViewBag.Pvideos = PlaylistVideo.GetVideos();
            return View("Dashboard");
        }
    }
}

