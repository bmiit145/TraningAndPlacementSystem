// field as  id , name , description and course id (FK)
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Data.SqlClient;
using TPS.Controllers;

namespace TPS.Models
{
    public class Playlist
    {
        public int ID { get; set; }

        [Required]
        public string name { get; set; }

        [AllowNull]
        public string description { get; set; }

        [Required]
        public int courseID { get; set; }

        public string courseName { get; set; }

        public int totalVideos { get; set; }

    
    public static Playlist GetPlaylist(int id){
        Playlist playlist = new Playlist();
        DbController db = new DbController();
        SqlCommand cmd;
        db.open();
        string query = "SELECT * FROM Playlists WHERE ID = @id";
        cmd = new SqlCommand(query, db.conn);
        cmd.Parameters.AddWithValue("@id", id);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            playlist.ID = reader.GetInt32(0);
            playlist.name = reader.GetString(1);
            playlist.description = reader.GetString(2);
            playlist.courseID = reader.GetInt32(3);
        }
        db.close();
        return playlist;
    }

    public static  List<Playlist> GetPlaylists()
    {
        List<Playlist> playlists = new List<Playlist>();
        DbController db = new DbController();
        SqlCommand cmd;
        db.open();
        // get course details also
        string query = "SELECT p.ID, p.Name, p.Description, p.CourseID, c.Name AS courseName " +
                       "FROM Playlists p " +
                       "LEFT JOIN Courses c ON p.CourseID = c.ID " +
                        "ORDER BY p.ID DESC";
        cmd = new SqlCommand(query, db.conn);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Playlist playlist = new Playlist();
            playlist.ID = reader.GetInt32(0);
            playlist.name = reader.GetString(1);
            playlist.description = reader.GetString(2);
            playlist.courseID = reader.GetInt32(3);
            playlist.courseName = reader.GetString(4);
            playlist.totalVideos = PlaylistVideo.GetVideoByPlaylist(playlist.ID).Count;

            playlists.Add(playlist);
        }
        db.close();
        return playlists;
    }
    }
}