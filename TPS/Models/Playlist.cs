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

    

    public static  List<Playlist> GetPlaylists()
    {
        List<Playlist> playlists = new List<Playlist>();
        DbController db = new DbController();
        SqlCommand cmd;
        db.open();
        string query = "SELECT * FROM Playlists";
        cmd = new SqlCommand(query, db.conn);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Playlist playlist = new Playlist();
            playlist.ID = reader.GetInt32(0);
            playlist.name = reader.GetString(1);
            playlist.description = reader.GetString(2);
            playlist.courseID = reader.GetInt32(3);
            playlists.Add(playlist);
        }
        db.close();
        return playlists;
    }
    }
}