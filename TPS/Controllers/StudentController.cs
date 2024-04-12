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

        [ActionName("viewPlaylist")]
        public IActionResult viewPlaylist()
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "0")
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            // get playlist details
            ViewBag.Playlists = Playlist.GetPlaylists();
            // get all the videos

            //ViewBag.Pvideos = PlaylistVideo.GetVideos(id);
            return View("Playlist");
        }

        [ActionName("ViewPlaylistVideo")]
        public IActionResult ViewPlaylistVideo(int? id, int? course)
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "0")
            {
                return RedirectToAction("SignIn", "Authentication");
            }


            ViewBag.Playlists = Playlist.GetPlaylists();
            ViewBag.Courses = Course.GetCourses();
            // get all course videos
            if (course > 0)
            {
                ViewBag.Pvideos = PlaylistVideo.GetVideos().FindAll(video => video.courseID == course);
            }
            else if (id > 0)
            {
                ViewBag.Playlist = Playlist.GetPlaylist(id ?? 0);
                ViewBag.Pvideos = PlaylistVideo.GetVideos(id);
            }
            else
            {
                ViewBag.Pvideos = PlaylistVideo.GetVideos();
            }

            return View("Videos");
        }

    }
}

