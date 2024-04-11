using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TPS.Models;
using TPS.Validation;
using System.ComponentModel;


namespace TPS.Controllers
{
    public class HiringController : Controller
    {
        DbController db = new DbController();

        [ActionName("Index")]
        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand command = new SqlCommand("SELECT * FROM Courses", db.conn);
            SqlDataReader reader = command.ExecuteReader();
            List<Course> courses = new List<Course>();
            while (reader.Read())
            {
                Course course = new()
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                };
                courses.Add(course);
            }
            ViewBag.courses = courses;
            reader.Close();
            // write a join query to get all the hiring programs and their course names
            // SqlCommand sqlCommand = new SqlCommand("SELECT hp.id,hp.program_name,hp.description,hp.start_date,hp.end_date,c.name AS course_name FROM dbo.HiringProgram hp INNER JOIN dbo.Courses c ON hp.course_id = c.id; ", db.conn);
            SqlCommand sqlCommand = new SqlCommand("SELECT hp.id,hp.program_name,hp.description,hp.start_date,hp.end_date,c.name AS course_name,c.id AS course_id FROM dbo.HiringProgram hp INNER JOIN dbo.Courses c ON hp.course_id = c.id; ", db.conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<Hiring> hirings = new List<Hiring>();
            while (sqlDataReader.Read())
            {
                Hiring hiring = new()
                {
                    Id = sqlDataReader.GetInt32(0),
                    Program_name = sqlDataReader.GetString(1),
                    Description = sqlDataReader.GetString(2),
                    Start_date = sqlDataReader.GetDateTime(3),
                    End_date = sqlDataReader.GetDateTime(4),
                    Course_name = sqlDataReader.GetString(5),
                    Course_id = sqlDataReader.GetInt32(6)
                };
                hirings.Add(hiring);
            }
            ViewBag.HiringPrograms = hirings;
            sqlDataReader.Close();
            return View("Index");
        }

        public ActionResult addHiringProgram(string program_name, string description, DateTime start_date, DateTime end_date, int course_id, string id, string edit)
        {

            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            if (edit == "true")
            {
                SqlCommand command = new SqlCommand("UPDATE HiringProgram SET program_name = @program_name,description = @description,start_date = @start_date,end_date = @end_date,course_id = @course_id WHERE id = @id", db.conn);
                command.Parameters.AddWithValue("@program_name", program_name);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@start_date", start_date);
                command.Parameters.AddWithValue("@end_date", end_date);
                command.Parameters.AddWithValue("@course_id", course_id);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            else
            {
                SqlCommand command = new SqlCommand("INSERT INTO HiringProgram (program_name,description,start_date,end_date,course_id) VALUES (@program_name,@description,@start_date,@end_date,@course_id)", db.conn);
                command.Parameters.AddWithValue("@program_name", program_name);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@start_date", start_date);
                command.Parameters.AddWithValue("@end_date", end_date);
                command.Parameters.AddWithValue("@course_id", course_id);
                command.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
        }

        [ActionName("deleteHiringProgram")]
        public IActionResult deleteHiringProgram(string id)
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand command = new SqlCommand("DELETE FROM HiringProgram WHERE id = @id", db.conn);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        [ActionName("CompanyHiring")]
        public IActionResult CompanyHiring()
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand sqlCommand = new SqlCommand("SELECT ch.id,ch.company_id,ch.hiring_id,ch.max_apply,c.company_name AS company_name,hp.program_name FROM dbo.CompanyHiring ch INNER JOIN dbo.CompanyProfile c ON ch.company_id = c.company_id INNER JOIN dbo.HiringProgram hp ON ch.hiring_id = hp.id", db.conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<CompanyHiring> companyHirings = new List<CompanyHiring>();
            while (sqlDataReader.Read())
            {
                CompanyHiring companyHiring = new()
                {
                    Id = sqlDataReader.GetInt32(0),
                    Company_id = sqlDataReader.GetInt32(1),
                    Hiring_id = sqlDataReader.GetInt32(2),
                    Max_apply = sqlDataReader.GetInt32(3),
                    Company_name = sqlDataReader.GetString(4),
                    Hiring_name = sqlDataReader.GetString(5)
                };
                companyHirings.Add(companyHiring);
            }
            ViewBag.CompanyHirings = companyHirings;
            sqlDataReader.Close();
            SqlCommand command = new SqlCommand("SELECT * FROM HiringProgram", db.conn);
            SqlDataReader reader = command.ExecuteReader();
            List<Hiring> hirings = new List<Hiring>();
            while (reader.Read())
            {
                Hiring hiring = new()
                {
                    Id = reader.GetInt32(0),
                    Program_name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Start_date = reader.GetDateTime(3),
                    End_date = reader.GetDateTime(4),
                    Course_id = reader.GetInt32(5)
                };
                hirings.Add(hiring);
            }
            ViewBag.HiringPrograms = hirings;
            reader.Close();
            /*SELECT TOP (1000) [company_id]
      ,[company_name]
      ,[industry_type]
      ,[email]
      ,[company_description]
      ,[img_url]
  FROM [dbo].[CompanyProfile] */
            SqlCommand command1 = new SqlCommand("SELECT * FROM CompanyProfile", db.conn);
            SqlDataReader reader1 = command1.ExecuteReader();
            List<Company> companies = new List<Company>();
            while (reader1.Read())
            {
                Company company = new()
                {
                    Id = reader1.GetInt32(0),
                    Name = reader1.GetString(1),
                    IndustryType = reader1.GetString(2),
                    Email = reader1.GetString(3),
                    Description = reader1.GetString(4),
                };
                companies.Add(company);
            }
            ViewBag.companies = companies;
            return View("AllCompanyHiring");
        }
        // addHiringProgram
        public ActionResult addCompanyHiring(int company_id, int hiring_id, int max_apply, string id, string edit)
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            if (edit == "true")
            {
                SqlCommand command = new SqlCommand("UPDATE CompanyHiring SET company_id = @company , hiring_id = @hiring , max_apply = @max_apply WHERE id = @id", db.conn);
                command.Parameters.AddWithValue("@company", company_id);
                command.Parameters.AddWithValue("@hiring", hiring_id);
                command.Parameters.AddWithValue("@max_apply", max_apply);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                return RedirectToAction("CompanyHiring");
            }
            else
            {
                SqlCommand command = new SqlCommand("INSERT INTO CompanyHiring (company_id,hiring_id,max_apply) VALUES (@company,@hiring,@max_apply)", db.conn);
                command.Parameters.AddWithValue("@company", company_id);
                command.Parameters.AddWithValue("@hiring", hiring_id);
                command.Parameters.AddWithValue("@max_apply", max_apply);
                command.ExecuteNonQuery();
                return RedirectToAction("CompanyHiring");
            }
        }

        [ActionName("deleteCompanyHiring")]
        public IActionResult deleteCompanyHiring(string id)
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand command = new SqlCommand("DELETE FROM CompanyHiring WHERE id = @id", db.conn);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            return RedirectToAction("CompanyHiring");
        }
    }
}

internal class CompanyHiring
{
    public int Id { get; set; }
    public int Company_id { get; set; }
    public int Hiring_id { get; set; }
    public int Max_apply { get; set; }
    public string Company_name { get; set; }
    public string Hiring_name
    {
        get; set;
    }
}

