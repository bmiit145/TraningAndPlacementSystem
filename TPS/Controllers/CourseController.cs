using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TPS.Models;
using System.Data.SqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPS.Controllers
{
    public class CourseController : Controller
    {
        DbController db = new DbController();

        [ActionName("Index")]
        public IActionResult Index()
        {
            ViewBag.Courses = Course.GetCourses();
            return View();

        }

        [ActionName("addOrEditCourse")]
        public IActionResult addOrEditCourse(int? id, string course_name , string description)
        {
;


            // add course to database if id = null  or id = 0
            if ( id == 0 || id == null)
            {

                // name should be unique
                db.open();
                string query = "SELECT * FROM Courses WHERE name = @Name";
                SqlCommand cmd = new SqlCommand(query, db.conn);
                cmd.Parameters.AddWithValue("@Name", course_name);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ViewBag.error = "Course name already exists";
                    ViewBag.Courses = Course.GetCourses();
                    return View("Index");
                }
                db.close();


                db.open();
                string query1 = "INSERT INTO Courses (name, description) VALUES (@Name, @Description)";
                SqlCommand cmd1 = new SqlCommand(query1, db.conn);
                cmd1.Parameters.AddWithValue("@Name", course_name);
                cmd1.Parameters.AddWithValue("@Description", description);
                cmd1.ExecuteNonQuery();
                db.close();

                ViewBag.success = "Course added successfully";
                ViewBag.Courses = Course.GetCourses();
                return View("Index");
            }
            else
            {

                // update course in database if id is not null
                db.open();
                string query2 = "UPDATE Courses SET name = @Name, description = @Description WHERE id = @ID";
                SqlCommand cmd2 = new SqlCommand(query2, db.conn);
                cmd2.Parameters.AddWithValue("@ID", id);
                cmd2.Parameters.AddWithValue("@Name", course_name);
                cmd2.Parameters.AddWithValue("@Description", description);
                cmd2.ExecuteNonQuery();
                db.close();

                ViewBag.success = "Course updated successfully";
                ViewBag.Courses = Course.GetCourses();
                return View("Index");
            }
        }
    }
}

