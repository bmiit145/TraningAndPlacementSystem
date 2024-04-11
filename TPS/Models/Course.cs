
using System.Data.SqlClient;
using TPS.Controllers;

namespace TPS.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        DbController db = new DbController();
        //function to get all courses
        public static List<Course> GetCourses()
        {
            List<Course> courses = new List<Course>();
            DbController db = new DbController();
            SqlCommand cmd;
            db.open();
            string query = "SELECT * FROM Courses";
            cmd = new SqlCommand(query, db.conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Course course = new Course();
                course.ID = reader.GetInt32(0);
                course.Name = reader.GetString(1);
                course.Description = reader.GetString(2);
                courses.Add(course);
            }
            db.close();
            return courses;
        }
    }
}