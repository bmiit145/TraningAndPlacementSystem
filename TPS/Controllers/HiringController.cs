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
using System.Net.Mail;
using System.Net;


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

        // InterviewDetails
        [ActionName("InterviewDetails")]
        public IActionResult InterviewDetails(int id)
        {
            // this will get the all details of the company like a company profile, company hiring, hiring program, interview
            if (HttpContext.Session.GetString("role") == null)
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            // get the all details for the interview using join query so we can show the company details,hiring program details and training program details
            SqlCommand command = new SqlCommand("SELECT i.interview_id,ch.company_id,c.company_name,i.interview_date,i.interview_time,i.venue,ch.id,hp.program_name,hp.description,hp.start_date,hp.end_date,c1.name AS course_name, c1.id AS course_id FROM dbo.Interview i INNER JOIN dbo.CompanyHiring ch ON i.company_hiring_id = ch.id INNER JOIN dbo.CompanyProfile c ON ch.company_id = c.company_id INNER JOIN dbo.HiringProgram hp ON ch.hiring_id = hp.id INNER JOIN dbo.Courses c1 ON hp.course_id = c1.id WHERE i.interview_id = @id", db.conn);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            TempInterViewDetails tempInterViewDetails = new TempInterViewDetails();
            while (reader.Read())
            {
                tempInterViewDetails.Interview_id = id;
                tempInterViewDetails.Company_id = reader.GetInt32(1);
                tempInterViewDetails.Company_name = reader.GetString(2);
                tempInterViewDetails.Interview_date = DateOnly.FromDateTime(reader.GetDateTime(3));
                tempInterViewDetails.Interview_time = reader.GetTimeSpan(4);
                tempInterViewDetails.Venue = reader.GetString(5);
                tempInterViewDetails.Company_hiring_id = reader.GetInt32(6);
                tempInterViewDetails.Program_name = reader.GetString(7);
                tempInterViewDetails.Description = reader.GetString(8);
                tempInterViewDetails.Start_date = reader.GetDateTime(9);
                tempInterViewDetails.End_date = reader.GetDateTime(10);
                tempInterViewDetails.Course_name = reader.GetString(11);
                tempInterViewDetails.Course_id = reader.GetInt32(12);
            }
            ViewBag.InterviewDetails = tempInterViewDetails;
            reader.Close();
            return View("InterviewDetails");
        }
        // ApplyInterviewStudent
        [ActionName("ApplyInterviewStudent")]
        public IActionResult ApplyInterviewStudent(int id)
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "0")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            // check of the student profile is approved or not
            SqlCommand checkCommandIS = new SqlCommand("SELECT is_approved FROM StudentProfile WHERE id = @id", db.conn);
            checkCommandIS.Parameters.AddWithValue("@id", HttpContext.Session.GetString("id"));
            int is_approved = (int)checkCommandIS.ExecuteScalar();
            if (is_approved == 0)
            {
                ViewBag.Error = "Your profile is not approved yet";
                return RedirectToAction("Profile", "Authentication");
            }
            // get the student id from the session
            int student_id = Convert.ToInt32(HttpContext.Session.GetString("id"));
            // check if the student has already applied for the interview
            SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM StudentInterview WHERE student_id = @student_id AND interview_id = @interview_id", db.conn);
            checkCommand.Parameters.AddWithValue("@student_id", student_id);
            checkCommand.Parameters.AddWithValue("@interview_id", id);
            int count = (int)checkCommand.ExecuteScalar();
            if (count > 0)
            {
                ViewBag.Error = "You have already applied for this interview";
                return RedirectToAction("Profile", "Authentication");
            }
            // check that if the max apply limit is reached or not
            SqlCommand checkCommand1 = new SqlCommand("SELECT max_apply FROM CompanyHiring WHERE id = (SELECT company_hiring_id FROM Interview WHERE interview_id = @interview_id)", db.conn);
            checkCommand1.Parameters.AddWithValue("@interview_id", id);
            int max_apply = (int)checkCommand1.ExecuteScalar();
            SqlCommand checkCommand2 = new SqlCommand("SELECT COUNT(*) FROM StudentInterview WHERE interview_id = @interview_id", db.conn);
            checkCommand2.Parameters.AddWithValue("@interview_id", id);
            int count1 = (int)checkCommand2.ExecuteScalar();
            if (count1 >= max_apply)
            {
                ViewBag.Error = "The max apply limit is reached for this interview";
                return RedirectToAction("Profile", "Authentication");
            }
            // apply for the interview
            SqlCommand command = new SqlCommand("INSERT INTO StudentInterview (student_id,interview_id,status) VALUES (@student_id,@interview_id,0)", db.conn);
            command.Parameters.AddWithValue("@student_id", student_id);
            command.Parameters.AddWithValue("@interview_id", Convert.ToInt32(id));
            command.ExecuteNonQuery();
            ViewBag.Success = "You have successfully applied for the interview";
            return RedirectToAction("Profile", "Authentication");
        }

        [ActionName("AppliedInterviewsStudent")]
        public IActionResult AppliedInterviewsStudent()
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "0")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            int student_id = Convert.ToInt32(HttpContext.Session.GetString("id"));
            // get all the applied interviews for the student with status,remarks and interview details
            SqlCommand sqlCommand = new SqlCommand("SELECT si.id,si.status,si.remark,si.interview_id,c.company_id,c.company_name,ch.id,hp.program_name FROM dbo.StudentInterview si INNER JOIN dbo.Interview i ON si.interview_id = i.interview_id INNER JOIN dbo.CompanyHiring ch ON i.company_hiring_id = ch.id INNER JOIN dbo.CompanyProfile c ON ch.company_id = c.company_id INNER JOIN dbo.HiringProgram hp ON ch.hiring_id = hp.id WHERE si.student_id = @student_id", db.conn);
            sqlCommand.Parameters.AddWithValue("@student_id", student_id);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<InterViewListStudent> tempInterViewDetails = new List<InterViewListStudent>();
            while (reader.Read())
            {
                InterViewListStudent tempAppliedWithStatus = new()
                {
                    Id = reader.GetInt32(0),
                    Status = reader.GetInt32(1),
                    // if remark is null then set it to empty string
                    Remark = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Interview_id = reader.GetInt32(3),
                    Company_id = reader.GetInt32(4),
                    Company_name = reader.GetString(5),
                    Company_hiring_id = reader.GetInt32(6),
                    Program_name = reader.GetString(7)
                };
                tempInterViewDetails.Add(tempAppliedWithStatus);
            }
            ViewBag.AppliedInterviews = tempInterViewDetails;
            reader.Close();
            return View("AppliedInterviewsStudent");
        }

        [ActionName("InterviesAppliedAdmin")]
        public IActionResult InterviesAppliedAdmin()
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            // get all the applied interviews for the admin with full students details and interview details
//             SELECT TOP (1000) [id]
//       ,[enro]
//       ,[fname]
//       ,[lname]
//       ,[email]
//       ,[contact_no]
//       ,[CGPA]
//       ,[resume]
//       ,[marks9]
//       ,[marks10]
//       ,[marks11]
//       ,[marks12]
//       ,[is_approved]
//   FROM [dbo].[StudentProfile]
            // SqlCommand sqlCommand = new SqlCommand("SELECT si.id,si.status,si.remark,si.interview_id,c.company_id,c.company_name,i.interview_date,i.interview_time,i.venue,ch.id,hp.program_name,s.id AS student_id,s.enro,s.fname,s.lname,s.email,s.contact_no,s.CGPA,s.resume,s.marks9,s.marks10,s.marks11,s.marks12,s.is_approved FROM dbo.StudentInterview si INNER JOIN dbo.Interview i ON si.interview_id = i.interview_id INNER JOIN dbo.CompanyHiring ch ON i.company_hiring_id = ch.id INNER JOIN dbo.CompanyProfile c ON ch.company_id = c.company_id INNER JOIN dbo.HiringProgram hp ON ch.hiring_id = hp.id INNER JOIN dbo.StudentProfile s ON si.student_id = s.id", db.conn);
            SqlCommand sqlCommand = new SqlCommand("SELECT si.id,si.status,si.remark,si.interview_id,c.company_id,c.company_name,i.interview_date,i.interview_time,i.venue,ch.id,hp.program_name,s.id AS student_id,s.enro,s.fname,s.lname,s.email,s.contact_no,s.CGPA,s.resume,s.marks9,s.marks10,s.marks11,s.marks12,s.is_approved,hp.program_name FROM dbo.StudentInterview si INNER JOIN dbo.Interview i ON si.interview_id = i.interview_id INNER JOIN dbo.CompanyHiring ch ON i.company_hiring_id = ch.id INNER JOIN dbo.CompanyProfile c ON ch.company_id = c.company_id INNER JOIN dbo.HiringProgram hp ON ch.hiring_id = hp.id INNER JOIN dbo.StudentProfile s ON si.student_id = s.id", db.conn);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<TempAppliedWithStatusAdmin> tempInterViewDetails = new List<TempAppliedWithStatusAdmin>();
            while (reader.Read())
            {
                // An exception of type 'System.InvalidCastException' occurred in System.Data.SqlClient.dll but was not handled in user code: 'Unable to cast object of type 'System.Int64' to type 'System.String'.'

                TempAppliedWithStatusAdmin tempAppliedWithStatus = new()
                {
                    // Id = reader.GetInt32(0),
                    // Status = reader.GetInt32(1),
                    // Remark = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    // Interview_id = reader.GetInt32(3),
                    // Company_id = reader.GetInt32(4),
                    // Company_name = reader.GetString(5),
                    // Interview_date = DateOnly.FromDateTime(reader.GetDateTime(6)),
                    // Interview_time = reader.GetTimeSpan(7),
                    // Venue = reader.GetString(8),
                    // Company_hiring_id = reader.GetInt32(9),
                    // Program_name = reader.GetString(10),
                    // Student_id = reader.GetInt32(11),
                    // Enro = reader.GetInt64(12).ToString(),
                    // Fname = reader.GetString(13),
                    // Lname = reader.GetString(14),
                    // Email = reader.GetString(15),
                    // Contact_no = reader.GetString(16),
                    // CGPA = reader.GetDouble(17),
                    // Resume = reader.GetString(18),
                    // Marks9 = reader.GetDouble(19),
                    // Marks10 = reader.GetDouble(20),
                    // Marks11 = reader.GetDouble(21),
                    // Marks12 = reader.GetDouble(22),
                    // Is_approved = reader.GetInt32(23)
                    // make all the fields nullable
                    Id = reader.GetInt32(0),
                    Status = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    Remark = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Interview_id = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                    Company_id = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Company_name = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Interview_date = reader.IsDBNull(6) ? new DateOnly() : DateOnly.FromDateTime(reader.GetDateTime(6)),
                    Interview_time = reader.IsDBNull(7) ? new TimeSpan() : reader.GetTimeSpan(7),
                    Venue = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    Company_hiring_id = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                    Program_name = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    Student_id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                    Enro = reader.IsDBNull(12) ? string.Empty : reader.GetInt64(12).ToString(),
                    Fname = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                    Lname = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                    Email = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                    Contact_no = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                    CGPA = reader.IsDBNull(17) ? 0 : reader.GetDouble(17),
                    Resume = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                    Marks9 = reader.IsDBNull(19) ? 0 : reader.GetDouble(19),
                    Marks10 = reader.IsDBNull(20) ? 0 : reader.GetDouble(20),
                    Marks11 = reader.IsDBNull(21) ? 0 : reader.GetDouble(21),
                    Marks12 = reader.IsDBNull(22) ? 0 : reader.GetDouble(22),
                    Is_approved = reader.IsDBNull(23) ? 0 : reader.GetInt32(23),
                    Hiring_name = reader.IsDBNull(5) ? string.Empty : reader.GetString(24)
                };
                tempInterViewDetails.Add(tempAppliedWithStatus);
            }
            ViewBag.AppliedInterviews = tempInterViewDetails;
            reader.Close();
            return View("InterviesAppliedAdmin");
        }

        // AddRemarks
        [ActionName("AddRemarks")]
        public IActionResult AddRemarks(int id, string remark)
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand command = new SqlCommand("UPDATE StudentInterview SET remark = @remark WHERE id = @id", db.conn);
            command.Parameters.AddWithValue("@remark", remark);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            return RedirectToAction("InterviesAppliedAdmin");
        }

        [ActionName("ApproveInterview")]
        public IActionResult ApproveInterview(int id)
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand command = new SqlCommand("UPDATE StudentInterview SET status = 1 WHERE id = @id", db.conn);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            // send email to the student that his interview is approved with remarks
            SqlCommand command1 = new SqlCommand("SELECT s.email FROM StudentProfile s INNER JOIN StudentInterview si ON s.id = si.student_id WHERE si.id = @id", db.conn);
            command1.Parameters.AddWithValue("@id", id);
            string email = (string)command1.ExecuteScalar();
            SqlCommand command2 = new SqlCommand("SELECT si.remark FROM StudentInterview si WHERE si.id = @id", db.conn);
            command2.Parameters.AddWithValue("@id", id);
            string remarks = (string)command2.ExecuteScalar();
            SendApprovalEmailInterview(email, remarks);
            return RedirectToAction("InterviesAppliedAdmin");
        }

        private void SendApprovalEmailInterview(string? email, string? remarks)
        {
            Console.WriteLine("Sending email to " + email);
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(email);
                mail.To.Add(email);
                mail.Subject = "Interview Approval";
                mail.Body = "Your interview is approved with remarks: " + remarks + " .";

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("21bmiit145@gmail.com", "nokwrtgzldqipgbv");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        [ActionName("RejectInterview")]
        public IActionResult RejectInterview(int id)
        {
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand command = new SqlCommand("UPDATE StudentInterview SET status = 2 WHERE id = @id", db.conn);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            // send email to the student that his interview is rejected with remarks
            SqlCommand command1 = new SqlCommand("SELECT s.email FROM StudentProfile s INNER JOIN StudentInterview si ON s.id = si.student_id WHERE si.id = @id", db.conn);
            command1.Parameters.AddWithValue("@id", id);
            string email = (string)command1.ExecuteScalar();
            SqlCommand command2 = new SqlCommand("SELECT si.remark FROM StudentInterview si WHERE si.id = @id", db.conn);
            command2.Parameters.AddWithValue("@id", id);
            string remarks = (string)command2.ExecuteScalar();
            SendRejectEmailInterview(email, remarks);
            return RedirectToAction("InterviesAppliedAdmin");
        }

        private void SendRejectEmailInterview(string? email, string? remarks)
        {
            Console.WriteLine("Sending email to " + email);
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(email);
                mail.To.Add(email);
                mail.Subject = "Interview Rejection";
                mail.Body = "Your interview is rejected with remarks: " + remarks + " .";

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("21bmiit145@gmail.com", "nokwrtgzldqipgbv");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
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

    internal class TempAppliedWithStatusAdmin
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public int Interview_id { get; set; }
        public int Company_id { get; set; }
        public string Company_name { get; set; }
        public DateOnly Interview_date { get; set; }
        public TimeSpan Interview_time { get; set; }
        public string Venue { get; set; }
        public int Company_hiring_id { get; set; }
        public string Program_name { get; set; }
        public int Student_id { get; set; }
        public string Enro { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Contact_no { get; set; }
        public double CGPA { get; set; }
        public string Resume { get; set; }
        public double Marks9 { get; set; }
        public double Marks10 { get; set; }
        public double Marks11 { get; set; }
        public double Marks12 { get; set; }
        public int Is_approved { get; set; }
        public string Hiring_name { get; set; }
    }

    internal class AppliedInterviews
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public int Interview_id { get; set; }
    }

    internal class TempAppliedWithStatus
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public int Interview_id { get; set; }
        public int Company_id { get; set; }
        public string Company_name { get; set; }
        public DateOnly Interview_date { get; set; }
        public TimeSpan Interview_time { get; set; }
        public string Venue { get; set; }
        public int Company_hiring_id { get; set; }
        public string Program_name { get; set; }
        public string Description { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public string Course_name { get; set; }
        public int Course_id { get; set; }
    }

    internal class TempInterViewDetails
    {
        public int Interview_id { get; set; }
        public int Company_id { get; set; }
        public string Company_name { get; set; }
        public DateOnly Interview_date { get; set; }
        public TimeSpan Interview_time { get; set; }
        public string Venue { get; set; }
        public int Company_hiring_id { get; set; }
        public string Program_name { get; set; }
        public string Description { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public string Course_name { get; set; }
        public int Course_id { get; set; }
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

