using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using TPS.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPS.Controllers
{
    public class AdminController : Controller
    {
        // GET: /<controller>/

        [ActionName("Index")]
        public IActionResult Index()
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            // get all the videos
            ViewBag.Pvideos = PlaylistVideo.GetVideos().Count();
            ViewBag.Playlists = Playlist.GetPlaylists().Count();

            // total Hiring Programs
            DbController db = new DbController();
            db.open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HiringProgram", db.conn);
            ViewBag.TotalHiringPrograms = Convert.ToInt32(cmd.ExecuteScalar());
            
            // count user where role = 0
            ViewBag.TotalStudents = Models.User.GetUsers().Where(u => u.Role == 0).Count();

            return View("Dashboard");
        }
    }
}

