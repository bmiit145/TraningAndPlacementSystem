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
        public IActionResult PlaylistPage(int? id)
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                ViewBag.Error = "You are not authorized to access this page";
                return RedirectToAction("SignIn", "Authentication");
            }
            if (id != null || id > 0)
            {
                ViewBag.Playlist = Playlist.GetPlaylist(id ?? 0);
                ViewBag.Videos = PlaylistVideo.GetVideoByPlaylist(id ?? 0);
            }
            
            ViewBag.Playlists = Playlist.GetPlaylists();
            ViewBag.Courses = Course.GetCourses();
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

    [ActionName("addorEditPlaylist")]
        public IActionResult addorEditPlaylist(string? ID , string name , string description , int course , string[] videoUrls , string[] videoTitles )
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                ViewBag.Error = "You are not authorized to access this page";
                return RedirectToAction("SignIn", "Authentication");
            }

            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || course == -1 || videoUrls.Length == 0 || videoTitles.Length == 0)
            {
                ViewBag.Error = "Name, Description, Course and atleast one video are required.";
                return RedirectToAction("playlist");
            }
            string[] link = videoUrls;
            for (int i = 0; i < link.Length; i++)
            {
                if (link[i].Contains("youtube.com"))
                {
                    link[i] = link[i].Substring(link[i].IndexOf("v=") + 2);
                    if (link[i].Contains("&"))
                    {
                        link[i] = link[i].Substring(0, link[i].IndexOf("&"));
                    }
                }
                else if (link[i].Contains("youtu.be"))
                {
                    link[i] = link[i].Substring(link[i].LastIndexOf("/") + 1);
                }
                else
                {
                    ViewBag.Error = "Invalid Youtube Video Link";
                    return View("playlist");
                }
            }

            if(ID == null)
            {
                db.open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Playlists (name , description , courseId) values (@name , @description , @courseId)", db.conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@courseId", course);
                cmd.ExecuteNonQuery();
                db.close();
                db.open();
                cmd = new SqlCommand("SELECT MAX(id) FROM Playlists", db.conn);
                int playlistId = (int)cmd.ExecuteScalar();
                    db.close();
                    db.open();
                    for (int i = 0; i < link.Length; i++)
                    {
                        cmd = new SqlCommand("INSERT INTO PlaylistVideos (link , title , playlistId , courseId) values (@link , @title , @playlistId , @courseId)", db.conn);
                        cmd.Parameters.AddWithValue("@link", link[i]);
                        cmd.Parameters.AddWithValue("@title", videoTitles[i]);
                        cmd.Parameters.AddWithValue("@playlistId", playlistId);
                        cmd.Parameters.AddWithValue("@courseId", course);
                        cmd.ExecuteNonQuery();
                    }
                    db.close();
                
                ViewBag.Success = "Playlist added successfully";
            }
            else
            {
                db.open();
                SqlCommand cmd = new SqlCommand("UPDATE Playlists SET name = @name , description = @description , courseId = @courseId WHERE id = @PlaylistId", db.conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@courseId", course);
                cmd.Parameters.AddWithValue("@PlaylistId", ID);
                cmd.ExecuteNonQuery();
                db.close();

                db.open();
                cmd = new SqlCommand("DELETE FROM PlaylistVideos WHERE playlistId = @PlaylistId", db.conn);
                cmd.Parameters.AddWithValue("@PlaylistId", ID);
                cmd.ExecuteNonQuery();
                db.close();

                db.open();
                for (int i = 0; i < link.Length; i++)
                {
                    cmd = new SqlCommand("INSERT INTO PlaylistVideos (link , title , playlistId , courseId) values (@link , @title , @playlistId , @courseId)", db.conn);
                    cmd.Parameters.AddWithValue("@link", link[i]);
                    cmd.Parameters.AddWithValue("@title", videoTitles[i]);
                    cmd.Parameters.AddWithValue("@playlistId", ID);
                    cmd.Parameters.AddWithValue("@courseId", course);
                    cmd.ExecuteNonQuery();
                }
                db.close();
                ViewBag.Success = "Playlist updated successfully";
            }

            return RedirectToAction("playlist");
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


        [ActionName("DeletePlaylist")]
        public IActionResult DeletePlaylist(int id)
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                ViewBag.Error = "You are not authorized to access this page";
                return RedirectToAction("SignIn", "Authentication");
            }
            // delete the videos from the database
            db.open();
            SqlCommand cmd1 = new SqlCommand("DELETE FROM PlaylistVideos WHERE playlistId = @PlaylistId", db.conn);
            cmd1.Parameters.AddWithValue("@PlaylistId", id);
            cmd1.ExecuteNonQuery();
            db.close();

            // delete the playlist from the database
            db.open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Playlists WHERE id = @PlaylistId", db.conn);
            cmd.Parameters.AddWithValue("@PlaylistId", id);
            cmd.ExecuteNonQuery();
            db.close();


            ViewBag.Success = "Playlist deleted successfully";
            return RedirectToAction("playlist");
        }
    }

}

