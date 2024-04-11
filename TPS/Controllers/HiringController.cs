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

        // all Interview
        [ActionName("AllInterview")]
        public IActionResult AllInterview()
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            /*SELECT TOP (1000) [interview_id]
      ,[company_hiring_id]
      ,[interview_date]
      ,[interview_time]
      ,[venue]
  FROM [dbo].[Interview] */
            // write a join that get interview_id,company_hiring_id, company name, hiring program name, interview date, interview time, venue
            SqlCommand sqlCommand = new SqlCommand("SELECT i.interview_id,ch.company_id,c.company_name,i.interview_date,i.interview_time,i.venue,ch.id FROM dbo.Interview i INNER JOIN dbo.CompanyHiring ch ON i.company_hiring_id = ch.id INNER JOIN dbo.CompanyProfile c ON ch.company_id = c.company_id", db.conn);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<Interview> interviews = new List<Interview>();
            while (sqlDataReader.Read())
            {
                Interview interview = new()
                {
                    Id = sqlDataReader.GetInt32(0),
                    Company_id = sqlDataReader.GetInt32(1),
                    Company_name = sqlDataReader.GetString(2),
                    Hiring_name = sqlDataReader.GetString(2),
                    Interview_date = DateOnly.FromDateTime(sqlDataReader.GetDateTime(3)),
                    Interview_time = sqlDataReader.GetTimeSpan(4),
                    Venue = sqlDataReader.GetString(5),
                    Company_hiring_id = sqlDataReader.GetInt32(6)
                };
                interviews.Add(interview);
            }
            ViewBag.Interviews = interviews;
            sqlDataReader.Close();
            // get company name and hiring program name from company hiring so that we can show it in the dropdown so we need to join company hiring and company profile and hiring program
            SqlCommand command = new SqlCommand("SELECT ch.id,c.company_name,hp.program_name FROM dbo.CompanyHiring ch INNER JOIN dbo.CompanyProfile c ON ch.company_id = c.company_id INNER JOIN dbo.HiringProgram hp ON ch.hiring_id = hp.id", db.conn);
            SqlDataReader reader = command.ExecuteReader();
            List<CompanyHiring> companyHirings = new List<CompanyHiring>();
            while (reader.Read())
            {
                CompanyHiring companyHiring = new()
                {
                    Id = reader.GetInt32(0),
                    Company_name = reader.GetString(1),
                    Hiring_name = reader.GetString(2)
                };
                companyHirings.Add(companyHiring);
            }
            ViewBag.CompanyHirings = companyHirings;
            reader.Close();
            return View("AllInterview");
        }
        // addInterview
        public ActionResult addInterview(int company_hiring_id, DateTime interview_date, DateTime interview_time, string venue, string id, string edit)
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            if (edit == "true")
            {
                SqlCommand command = new SqlCommand("UPDATE Interview SET company_hiring_id = @company_hiring_id , interview_date = @interview_date , interview_time = @interview_time , venue = @venue WHERE interview_id = @id", db.conn);
                command.Parameters.AddWithValue("@company_hiring_id", company_hiring_id);
                command.Parameters.AddWithValue("@interview_date", interview_date);
                command.Parameters.AddWithValue("@interview_time", interview_time);
                command.Parameters.AddWithValue("@venue", venue);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                return RedirectToAction("AllInterview");
            }
            else
            {
                SqlCommand command = new SqlCommand("INSERT INTO Interview (company_hiring_id,interview_date,interview_time,venue) VALUES (@company_hiring_id,@interview_date,@interview_time,@venue)", db.conn);
                command.Parameters.AddWithValue("@company_hiring_id", company_hiring_id);
                command.Parameters.AddWithValue("@interview_date", interview_date);
                command.Parameters.AddWithValue("@interview_time", interview_time);
                command.Parameters.AddWithValue("@venue", venue);
                command.ExecuteNonQuery();
                return RedirectToAction("AllInterview");
            }
        }
        // deleteInterview
        [ActionName("deleteInterview")]
        public IActionResult deleteInterview(string id)
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand command = new SqlCommand("DELETE FROM Interview WHERE interview_id = @id", db.conn);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            return RedirectToAction("AllInterview");
        }

        [ActionName("DetailsCompany")]
        public IActionResult DetailsCompany(int id)
        {
            // this will get the all details of the company like a company profile, company hiring, hiring program, interview
            if (HttpContext.Session.GetString("role") == null)
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            // get company profile
            SqlCommand command = new SqlCommand("SELECT * FROM CompanyProfile WHERE company_id = @id", db.conn);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            Company company = new Company()
            {
                Name = string.Empty,
                IndustryType = string.Empty,
                Email = string.Empty,
                Description = string.Empty
            };
            while (reader.Read())
            {
                company.Id = reader.GetInt32(0);
                company.Name = reader.GetString(1);
                company.IndustryType = reader.GetString(2);
                company.Email = reader.GetString(3);
                company.Description = reader.GetString(4);
            }
            ViewBag.company = company;
            reader.Close();
            // get company hiring details like program_name,details,start_date,end_date and course_name
            SqlCommand command1 = new SqlCommand("SELECT ch.id,hp.program_name,hp.description,hp.start_date,hp.end_date,c.name AS course_name FROM dbo.CompanyHiring ch INNER JOIN dbo.HiringProgram hp ON ch.hiring_id = hp.id INNER JOIN dbo.Courses c ON hp.course_id = c.id WHERE ch.company_id = @id", db.conn);
            command1.Parameters.AddWithValue("@id", id);
            SqlDataReader reader1 = command1.ExecuteReader();
            List<StudentCompanyHiring> studentCompanyHirings = new List<StudentCompanyHiring>();
            while (reader1.Read())
            {
                StudentCompanyHiring studentCompanyHiring = new()
                {
                    Id = reader1.GetInt32(0),
                    Program_name = reader1.GetString(1),
                    Description = reader1.GetString(2),
                    Start_date = reader1.GetDateTime(3),
                    End_date = reader1.GetDateTime(4),
                    Course_name = reader1.GetString(5)
                };
                studentCompanyHirings.Add(studentCompanyHiring);
            }
            ViewBag.CompanyHirings = studentCompanyHirings;
            reader1.Close();
            // get interview details
            SqlCommand command3 = new SqlCommand("SELECT i.interview_id,ch.company_id,c.company_name,i.interview_date,i.interview_time,i.venue,ch.id FROM dbo.Interview i INNER JOIN dbo.CompanyHiring ch ON i.company_hiring_id = ch.id INNER JOIN dbo.CompanyProfile c ON ch.company_id = c.company_id WHERE c.company_id = @id", db.conn);
            command3.Parameters.AddWithValue("@id", id);
            SqlDataReader reader3 = command3.ExecuteReader();
            List<Interview> interviews = new List<Interview>();
            while (reader3.Read())
            {
                Interview interview = new()
                {
                    Id = reader3.GetInt32(0),
                    Company_id = reader3.GetInt32(1),
                    Company_name = reader3.GetString(2),
                    Hiring_name = reader3.GetString(2),
                    Interview_date = DateOnly.FromDateTime(reader3.GetDateTime(3)),
                    Interview_time = reader3.GetTimeSpan(4),
                    Venue = reader3.GetString(5),
                    Company_hiring_id = reader3.GetInt32(6)
                };
                interviews.Add(interview);
            }
            ViewBag.Interviews = interviews;
            reader3.Close();
            return View("DetailsCompany");
        }

        internal class Interview
        {
            public int Id { get; set; }
            public int Company_id { get; set; }
            public string Company_name { get; set; }
            public string Hiring_name { get; set; }
            public DateOnly Interview_date { get; set; }
            public TimeSpan Interview_time { get; set; }
            public string Venue { get; set; }
            public int Company_hiring_id { get; set; }
        }
    }

    internal class StudentCompanyHiring
    {
        public int Id { get; set; }
        public string Program_name { get; set; }
        public string Description { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public string Course_name { get; set; }
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

