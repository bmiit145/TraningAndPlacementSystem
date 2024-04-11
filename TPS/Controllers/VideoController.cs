using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TPS.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPS.Controllers
{
    public class VideoController : Controller
    {
        // GET: /<controller>/

        DbController db = new DbController();
        [ActionName("VideoList")]
        public IActionResult VideoList(int? id)
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                ViewBag.Error = "You are not authorized to access this page";
                return RedirectToAction("SignIn", "Authentication");
            }

            // if id available than give that details
            if (id != null)
            {
                db.open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM PlaylistVideos WHERE id = @VideoId", db.conn);
                cmd.Parameters.AddWithValue("@VideoId", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ViewBag.Video = new PlaylistVideo(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.IsDBNull(3) ? null : (int?)reader.GetInt32(3), reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4));
                }
                reader.Close();
                db.close();
            }

            // pass cousrses and playlists to view
            ViewBag.Courses = Course.GetCourses();
            ViewBag.Playlists = Playlist.GetPlaylists();
            ViewBag.Videos = PlaylistVideo.GetVideos();
            return View("videoList");
        }

        [ActionName("Playlist")]
        public IActionResult PlaylistPage()
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                ViewBag.Error = "You are not authorized to access this page";
                return RedirectToAction("SignIn", "Authentication");
            }

            return View("playlist");
        }


        [ActionName("AddOrEditVideo")]
        public IActionResult AddOrEditVideo(int? id, string link, string title, bool optedInPlaylist, int? course, int? playlist)
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                ViewBag.Error = "You are not authorized to access this page";
                return RedirectToAction("SignIn", "Authentication");
            }

            // get a youtube videoID only from the link
            if (link.Contains("youtube.com"))
            {
                link = link.Substring(link.IndexOf("v=") + 2);
                if (link.Contains("&"))
                {
                    link = link.Substring(0, link.IndexOf("&"));
                }
            }
            else if (link.Contains("youtu.be"))
            {
                link = link.Substring(link.LastIndexOf("/") + 1);
            }
            else
            {
                ViewBag.Error = "Invalid Youtube Video Link";
                return View("videoList");
            }

            // save the video to the database with validation
            if (string.IsNullOrWhiteSpace(link) || string.IsNullOrWhiteSpace(title))
            {
                ViewBag.Error = "Video Link and Title are required.";
                return View("videoList");
            }

            if (optedInPlaylist && playlist == null && playlist == -1)
            {
                ViewBag.Error = "Playlist and Course are required if you want to add video to playlist.";
                return View("videoList");
            }

            if (!optedInPlaylist && course == null && course == -1)
            {
                ViewBag.Error = "Course is required if you want to add video to playlist.";
                return View("videoList");
            }

            PlaylistVideo video = new PlaylistVideo();


            if (id == null)
            {
                if (optedInPlaylist)
                {
                    // retrive course id
                    db.open();
                    SqlCommand cmd = new SqlCommand("SELECT CourseId FROM Playlist WHERE PlaylistId = @PlaylistId", db.conn);
                    cmd.Parameters.AddWithValue("@PlaylistId", playlist);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int courseId = reader.GetInt32(0);
                        reader.Close();
                        db.close();
                        db.open();
                        cmd = new SqlCommand("INSERT INTO PlaylistVideos (link , title , playlistId , courseId) values (@link , @title , @playlistId , @courseId)", db.conn);
                        cmd.Parameters.AddWithValue("@link", link);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@playlistId", playlist);
                        cmd.Parameters.AddWithValue("@courseId", courseId);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        ViewBag.Error = "Invaild Playlist";
                        return View("videoList");
                    }
                    reader.Close();
                    db.close();
                }
                else
                {
                    db.open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO PlaylistVideos (link , title , courseId) values (@link , @title , @courseId)", db.conn);
                    cmd.Parameters.AddWithValue("@link", link);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@courseId", course);
                    cmd.ExecuteNonQuery();
                    db.close();
                }

                ViewBag.Success = "Video added successfully";
                return RedirectToAction("VideoList");
            }
            else
            {
                int courseId = course ?? 0;
                int playlistId = playlist ?? 0;
                if (optedInPlaylist)
                {
                    db.open();
                    SqlCommand cmd1 = new SqlCommand("SELECT CourseId FROM Playlist WHERE PlaylistId = @PlaylistId", db.conn);
                    cmd1.Parameters.AddWithValue("@PlaylistId", playlist);
                    SqlDataReader reader = cmd1.ExecuteReader();
                    if (reader.Read())
                    {
                        courseId = reader.GetInt32(0);
                        reader.Close();
                        db.close();
                        db.open();

                    }
                    else
                    {
                        ViewBag.Error = "Invaild Playlist";
                        return View("videoList");
                    }
                    reader.Close();
                    db.close();
                }

                db.open();
                SqlCommand cmd = new SqlCommand("UPDATE PlaylistVideos SET link = @link , title = @title , courseId = @courseId , playlistId = @playlistId WHERE id = @id", db.conn);
                cmd.Parameters.AddWithValue("@link", link);
                cmd.Parameters.AddWithValue("@title", title);
                if (optedInPlaylist)
                {
                    cmd.Parameters.AddWithValue("@courseId", courseId);
                    cmd.Parameters.AddWithValue("@playlistId", playlistId);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@courseId", courseId);
                    cmd.Parameters.AddWithValue("@playlistId", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                db.close();

                ViewBag.Success = "Video updated successfully";
                return RedirectToAction("VideoList");

            }
        }

        [ActionName("DeleteVideo")]
        public IActionResult DeleteVideo(int id)
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                ViewBag.Error = "You are not authorized to access this page";
                return RedirectToAction("SignIn", "Authentication");
            }

            // delete the video from the database
            db.open();
            SqlCommand cmd = new SqlCommand("DELETE FROM PlaylistVideos WHERE id = @VideoId", db.conn);
            cmd.Parameters.AddWithValue("@VideoId", id);
            cmd.ExecuteNonQuery();
            db.close();

            ViewBag.Success = "Video deleted successfully";
            return RedirectToAction("VideoList");
        }

    }

}

