// model for the playlist video as id , link , title and playlist id (FK) and course id (FK)
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Data.SqlClient;
using TPS.Controllers;

namespace TPS.Models
{
    public class PlaylistVideo
    {
        public int ID { get; set; }

        [Required]
        public string link { get; set; }

        [AllowNull]
        public string title { get; set; }

        [Required]
        public int playlistID { get; set; }

        [Required]
        public int courseID { get; set; }

        public string playlistName { get; set; }
        public string courseName { get; set; }

        public PlaylistVideo()
        {
        }
        public PlaylistVideo(int ID, string link, string title, int? playlistID, int? courseID)
        {
            this.ID = ID;
            this.link = link;
            this.title = title;
            this.playlistID = playlistID ?? 0;
            this.courseID = courseID ?? 0;
        }

        public static List<PlaylistVideo> GetVideoByPlaylist(int id){
                return GetVideos().FindAll(video => video.playlistID == id);
        }

        // GetVideos 
       public static List<PlaylistVideo> GetVideos()
{
    List<PlaylistVideo> videos = new List<PlaylistVideo>();
    DbController db = new DbController();
    SqlCommand cmd;
    db.open();
    // short by playlist id
    string query = "SELECT pv.ID, pv.link, pv.title, pv.playlistID, pv.courseID, c.Name AS CourseName, p.Name AS PlaylistName " +
                   "FROM PlaylistVideos pv " +
                   "LEFT JOIN Courses c ON pv.courseID = c.ID " +
                   "LEFT JOIN Playlists p ON pv.playlistID = p.ID " +
                   "ORDER BY pv.playlistID";
    cmd = new SqlCommand(query, db.conn);
    SqlDataReader reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        PlaylistVideo video = new PlaylistVideo();
        video.ID = reader.GetInt32(0);
        video.link = reader.GetString(1);
        video.title = reader.GetString(2);
        if (!reader.IsDBNull(3))
        {
            video.playlistID = reader.GetInt32(3);
        }
        video.courseID = reader.GetInt32(4);

        // Add course name and playlist name to the video object
        video.courseName = reader.IsDBNull(5) ? null : reader.GetString(5);
        video.playlistName = reader.IsDBNull(6) ? null : reader.GetString(6);

        videos.Add(video);
    }
    db.close();
    return videos;
}

    }


}