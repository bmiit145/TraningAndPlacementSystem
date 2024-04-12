﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;

namespace TPS.Controllers
{
    public class AuthenticationController : Controller
    {
        DbController db = new DbController();

        [ActionName("SignIn")]
        public IActionResult SignIn()
        {
            db.InsertInitialAdminData("admin", "admin");

            //check for remember me cookie
            if (Request.Cookies["username"] != null && Request.Cookies["role"] != null)
            {
                // store username and role in session
                ISession Session = HttpContext.Session;
                Session.SetString("username", Request.Cookies["username"]);
                Session.SetString("role", Request.Cookies["role"]);
                if (Request.Cookies["role"] == "1")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Student");
                }
            }

            // check role from sessiom if session available then redirect to dashboard
            ISession session = HttpContext.Session;
            if (session.GetString("role") != null)
            {
                if (session.GetString("role") == "1")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Student");
                }
            }
            return View();
        }


        public ActionResult signinSubmit(string username, string password, bool rememberMe = false)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter a username and password.";
                return View("Signin");
            }


            db.open();
            SqlCommand cmd = new SqlCommand("select username , password , role from users where username = @username AND password = @password ", db.conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    // store username and role in session
                    ISession session = HttpContext.Session;
                    session.SetString("username", dr["username"].ToString());
                    session.SetString("role", dr["role"].ToString());

                    if (rememberMe)
                    {
                        // store username and role in cookie for remember me
                        Response.Cookies.Append("username", dr["username"].ToString());
                        Response.Cookies.Append("role", dr["role"].ToString());
                    }


                    if (dr["role"].ToString() == "1")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        // store student id in session
                        db.close();
                        db.open();
                        SqlCommand cmd1 = new SqlCommand("select id from users where username = @username", db.conn);
                        cmd1.Parameters.AddWithValue("@username", username);
                        SqlDataReader dr1 = cmd1.ExecuteReader();
                        if (dr1.HasRows)
                        {
                            while (dr1.Read())
                            {
                                session.SetString("id", dr1["id"].ToString());
                            }
                        }
                        return RedirectToAction("Index", "Student");
                    }
                }
            }
            else
            {
                ViewBag.Error = "Invalid username or password.";
                return View("Signin");
            }

            ViewBag.Error = "Something Wents Wrong!";
            return View("Signin");
        }

        // code for register and after register redirect to login page
        public ActionResult registerSubmit(string username, string password, string first_name, string last_name, string email, string c_no, string e_no)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(first_name) || string.IsNullOrEmpty(last_name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(c_no) || string.IsNullOrEmpty(e_no))
            {
                ViewBag.Error = "Please fillup All the Details !";
                return View("SignUp");
            }

            db.open();
            SqlCommand cmd = new SqlCommand("insert into users (username , password , role) values (@username , @password , @role)", db.conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@role", 0);
            cmd.ExecuteNonQuery();

            //  get inserted document id
            SqlCommand cmd1 = new SqlCommand("select id from users where username = @username", db.conn);
            cmd1.Parameters.AddWithValue("@username", username);
            SqlDataReader dr = cmd1.ExecuteReader();
            int id = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    id = Convert.ToInt32(dr["id"]);
                }
            }
            dr.Close();
            // now insert student profile
            SqlCommand cmd2 = new SqlCommand("insert into StudentProfile (id,enro,fname,lname,email,contact_no,is_approved) values (@id , @enro , @fname , @lname , @email , @contact_no , @is_approved)", db.conn);
            cmd2.Parameters.AddWithValue("@id", id);
            cmd2.Parameters.AddWithValue("@enro", e_no);
            cmd2.Parameters.AddWithValue("@fname", first_name);
            cmd2.Parameters.AddWithValue("@lname", last_name);
            cmd2.Parameters.AddWithValue("@email", email);
            cmd2.Parameters.AddWithValue("@contact_no", c_no);
            cmd2.Parameters.AddWithValue("@is_approved", 0);
            cmd2.ExecuteNonQuery();

            db.close();
            return RedirectToAction("SignIn");
        }


        [ActionName("Logout")]
        public IActionResult Logout()
        {
            ISession session = HttpContext.Session;
            session.Clear();
            Response.Cookies.Delete("username");
            Response.Cookies.Delete("role");
            return RedirectToAction("SignIn");
        }


        [ActionName("PasswordReset")]
        public IActionResult PasswordReset()
        {
            return View();
        }

        [ActionName("ApproveStudent")]
        public IActionResult ApproveStudent(int id)
        {
            // check that user is logged in or not
            ISession session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            // check that user is admin or not
            if (session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand cmd = new SqlCommand("update StudentProfile set is_approved = 1 where id = @id", db.conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            // send email to student
            SqlCommand cmd1 = new SqlCommand("select email from StudentProfile where id = @id", db.conn);
            cmd1.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cmd1.ExecuteReader();
            string email = "";
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    email = dr["email"].ToString();
                }
            }
            dr.Close();
            SendApprovalEmail(email);
            return RedirectToAction("StudentList");
        }

        private void SendApprovalEmail(string? email)
        {
            Console.WriteLine("Sending email to " + email);
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(email);
                mail.To.Add(email);
                mail.Subject = "Approval";
                mail.Body = "Your profile has been approved by admin.";

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("21bmiit145@gmail.com", "nokwrtgzldqipgbv");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        private void SendRejectionEmail(string? email)
        {
            Console.WriteLine("Sending email to " + email);
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(email);
                mail.To.Add(email);
                mail.Subject = "Approval";
                mail.Body = "Your profile has been rejected by admin.";

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("21bmiit145@gmail.com", "nokwrtgzldqipgbv");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        [ActionName("RejectStudent")]
        public IActionResult RejectStudent(int id)
        {
            // check that user is logged in or not
            ISession session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            // check that user is admin or not
            if (session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand cmd = new SqlCommand("update StudentProfile set is_approved = 2 where id = @id", db.conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            // send email to student
            SqlCommand cmd1 = new SqlCommand("select email from StudentProfile where id = @id", db.conn);
            cmd1.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cmd1.ExecuteReader();
            string email = "";
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    email = dr["email"].ToString();
                }
            }
            dr.Close();
            SendRejectionEmail(email);
            return RedirectToAction("StudentList");
        }

        [ActionName("DeleteStudent")]
        public IActionResult DeleteStudent(int id)
        {
            // check that user is logged in or not
            ISession session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            // check that user is admin or not
            if (session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand cmd = new SqlCommand("delete from StudentProfile where id = @id", db.conn);
            cmd.Parameters.AddWithValue("@id", id);
            // also delete user

            SqlCommand cmd1 = new SqlCommand("delete from users where id = @id", db.conn);
            cmd1.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            cmd1.ExecuteNonQuery();

            return RedirectToAction("StudentList");
        }

        [ActionName("ImprovementStudent")]
        public IActionResult ImprovementStudent(int id)
        {
            // check that user is logged in or not
            ISession session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            // check that user is admin or not
            if (session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            SqlCommand cmd = new SqlCommand("update StudentProfile set is_approved = 3 where id = @id", db.conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            return RedirectToAction("StudentList");
        }

        [ActionName("StudentList")]
        public IActionResult StudentList()
        {
            // check that user is logged in or not
            ISession session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            // check that user is admin or not
            if (session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            // select the student where the cgpa and marks are not null
            SqlCommand cmd = new SqlCommand("select * from StudentProfile where CGPA is not null and marks9 is not null and marks10 is not null and marks11 is not null and marks12 is not null", db.conn);
            SqlDataReader dr = cmd.ExecuteReader();
            List<Student> students = new List<Student>();
            while (dr.Read())
            {
                Student student = new Student()
                {
                    Id = Convert.ToInt32(dr["id"]),
                    Enro = dr["enro"].ToString(),
                    Fname = dr["fname"].ToString(),
                    Lname = dr["lname"].ToString(),
                    Email = dr["email"].ToString(),
                    Contact = dr["contact_no"].ToString(),
                    CGPA = dr["CGPA"].ToString(),
                    Marks9 = dr["marks9"].ToString(),
                    Marks10 = dr["marks10"].ToString(),
                    Marks11 = dr["marks11"].ToString(),
                    Marks12 = dr["marks12"].ToString(),
                    IsApproved = dr["is_approved"].ToString()
                };
                students.Add(student);
            }
            ViewBag.Students = students;
            return View();
        }

        public IActionResult ResetLinkSend(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                ViewBag.Error = "Please enter a username.";
                return View("PasswordReset");
            }

            db.open();
            using (SqlCommand cmd = new SqlCommand("SELECT id, username, role FROM users WHERE username = @username", db.conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (dr["role"].ToString() == "1")
                        {
                            ViewBag.Error = "Admin password reset is not allowed.";
                            return View("PasswordReset");
                        }
                        string id = dr["id"].ToString();
                        string username1 = dr["username"].ToString();

                        // Close the current reader before opening another one
                        dr.Close();

                        using (SqlCommand cmd1 = new SqlCommand("SELECT email FROM StudentProfile WHERE id = @id", db.conn))
                        {
                            cmd1.Parameters.AddWithValue("@id", id);
                            using (SqlDataReader dr1 = cmd1.ExecuteReader())
                            {
                                if (dr1.Read())
                                {
                                    string email = dr1["email"].ToString();
                                    // Generate token of 32 characters
                                    string token = Guid.NewGuid().ToString().Replace("-", "");
                                    string resetUrl = $"{this.Request.Scheme}://{this.Request.Host}/Authentication/PasswordChangeRequest?token={token}&username={username1}";

                                    dr1.Close();
                                    // Save token in the database
                                    using (SqlCommand cmd2 = new SqlCommand("INSERT INTO PasswordResetToken (username, token) VALUES (@username, @token)", db.conn))
                                    {
                                        cmd2.Parameters.AddWithValue("@username", username1);
                                        cmd2.Parameters.AddWithValue("@token", token);
                                        cmd2.ExecuteNonQuery();
                                    }
                                    SendResetPasswordEmail(email, resetUrl);
                                    ViewBag.Success = "Reset link has been sent to your email.";
                                    return View("PasswordReset");
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Invalid username.";
                        return View("PasswordReset");
                    }
                }
            }

            ViewBag.Error = "Something went wrong!";
            return View("PasswordReset");
        }



        protected void SendResetPasswordEmail(string recipientEmail, string resetUrl)
        {
            Console.WriteLine("Sending email to " + recipientEmail);
            Console.WriteLine("Reset URL: " + resetUrl);
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("21bmiit145@gmail.com");
                mail.To.Add(recipientEmail);
                mail.Subject = "Reset Your Password";
                mail.Body = $"Please click the following link to reset your password: {resetUrl}";

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("21bmiit145@gmail.com", "nokwrtgzldqipgbv");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }


        [ActionName("PasswordChangeRequest")]
        public IActionResult PasswordChangeRequest(string token, string username)
        {
            db.open();
            SqlCommand cmd = new SqlCommand("select username from PasswordResetToken where token = @token", db.conn);
            cmd.Parameters.AddWithValue("@token", token);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (dr["username"].ToString() == username)
                    {
                        ViewBag.Token = token;
                        ViewBag.Username = username;
                        return View("PasswordChange");
                    }
                }
            }
            ViewBag.Error = "Invalid token.";
            return View("PasswordReset");
        }

        [ActionName("ChangePassword")]
        public IActionResult ChangePassword(string old_password, string new_password, string con_password)
        {

            ISession session = HttpContext.Session;
            string username = session.GetString("username");


            if (string.IsNullOrEmpty(old_password))
            {
                ViewBag.Error = "Please enter a Old password.";
                return View("Profile");
            }

            if (string.IsNullOrEmpty(new_password) || string.IsNullOrEmpty(con_password))
            {
                ViewBag.Error = "Please enter a password and confirm password.";
                return View("Profile");
            }

            if (String.IsNullOrEmpty(username))
            {
                ViewBag.Error = "Invalid username.";
                return View("Profile");
            }

            if (new_password != con_password)
            {
                ViewBag.Error = "Password and confirm password do not match.";
                return View("Profile");
            }


            // check old password
            db.close();
            db.open();
            SqlCommand cmd1 = new SqlCommand("select password from users where username = @username", db.conn);
            cmd1.Parameters.AddWithValue("@username", username);
            SqlDataReader dr = cmd1.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (dr["password"].ToString() != old_password)
                    {
                        ViewBag.Error = "Old password is incorrect.";
                        return View("Profile");
                    }
                }
            }
            else
            {
                ViewBag.Error = "Invalid username.";
                return View("Profile");
            }

            // update password
            db.close();
            db.open();
            SqlCommand cmd = new SqlCommand("update users set password = @password where username = @username", db.conn);
            cmd.Parameters.AddWithValue("@password", new_password);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.ExecuteNonQuery();

            ViewBag.Success = "Password has been updated.";
            return View("Profile");

        }

        [ActionName("UpdatePassword")]
        public IActionResult UpdatePassword(string username, string password, string con_password)
        {
            if (string.IsNullOrEmpty(username))
            {
                ViewBag.Error = "Unauthorised Access!";
                return View("PasswordChange");
            }
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(con_password))
            {
                ViewBag.Error = "Please enter a password and confirm password.";
                return View("PasswordChange");
            }

            if (String.IsNullOrEmpty(username))
            {
                ViewBag.Error = "Invalid username.";
                return View("PasswordChange");
            }

            if (password != con_password)
            {
                ViewBag.Error = "Password and confirm password do not match.";
                return View("PasswordChange");
            }

            db.close();
            db.open();
            SqlCommand cmd = new SqlCommand("update users set password = @password where username = @username", db.conn);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.ExecuteNonQuery();
            db.close();
            db.open();
            SqlCommand cmd1 = new SqlCommand("delete from PasswordResetToken where username = @username", db.conn);
            cmd1.Parameters.AddWithValue("@username", username);
            cmd1.ExecuteNonQuery();
            db.close();

            ViewBag.Success = "Password has been updated.";
            return RedirectToAction("SignIn", "Authentication");
        }



        [ActionName("Offline")]
        public ActionResult Offline()
        {
            return View("~/Views/Authentication/offline.cshtml");
        }


        [ActionName("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [ActionName("Profile")]
        public IActionResult Profile()
        {
            // check that user is logged in or not
            ISession session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            string username = session.GetString("username");
            db.open();
            SqlCommand cmd = new SqlCommand("select * from StudentProfile where id = (select id from users where username = @username)", db.conn);
            cmd.Parameters.AddWithValue("@username", username);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ViewBag.Username = username;
                    ViewBag.Firstname = dr["fname"].ToString();
                    ViewBag.Lastname = dr["lname"].ToString();
                    ViewBag.Email = dr["email"].ToString();
                    ViewBag.Contact = dr["contact_no"].ToString();
                    ViewBag.Enro = dr["enro"].ToString();
                    ViewBag.CGPA = dr["CGPA"].ToString();
                    ViewBag.MARKS9 = dr["MARKS9"].ToString();
                    ViewBag.MARKS10 = dr["MARKS10"].ToString();
                    ViewBag.MARKS11 = dr["MARKS11"].ToString();
                    ViewBag.MARKS12 = dr["MARKS12"].ToString();
                    ViewBag.IsApproved = dr["is_approved"].ToString();
                }
            }
            dr.Close();
            return View();
        }

        [ActionName("UpdateProfile")]
        public ActionResult UpdateProfile(string first_name, string last_name, string email, string phone, string cgpa, string marks9, string marks10, string marks11, string marks12, string is_approved)
        {
            // check that user is logged in or not
            ISession session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")))
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            string username = session.GetString("username");
            db.open();
            SqlCommand cmd = new SqlCommand("update StudentProfile set fname = @fname , lname = @lname , email = @email , contact_no = @contact_no , CGPA = @CGPA, marks9 = @marks9, marks10 = @marks10, marks11 = @marks11, marks12 = @marks12, is_approved = @is_approved where id = (select id from users where username = @username)", db.conn);
            cmd.Parameters.AddWithValue("@fname", first_name);
            cmd.Parameters.AddWithValue("@lname", last_name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@contact_no", phone);
            cmd.Parameters.AddWithValue("@CGPA", cgpa);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@marks9", marks9);
            cmd.Parameters.AddWithValue("@marks10", marks10);
            cmd.Parameters.AddWithValue("@marks11", marks11);
            cmd.Parameters.AddWithValue("@marks12", marks12);
            // if is_approved is not null then set it to 0
            if (string.IsNullOrEmpty(is_approved))
            {
                cmd.Parameters.AddWithValue("@is_approved", 0);
            }
            else
            {
                // if the user id_approved is 3 then set it to 4
                if (is_approved == "3")
                {
                    cmd.Parameters.AddWithValue("@is_approved", 4);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@is_approved", is_approved);
                }
            }
            cmd.ExecuteNonQuery();
            ViewBag.Success = "Profile has been updated and waiting for approval.";
            return RedirectToAction("Profile", "Authentication");
        }


        [ActionName("SignUpCover")]
        public IActionResult SignUpCover()
        {
            return View();
        }

        [ActionName("PasswordChangeBasic")]
        public IActionResult PasswordChangeBasic()
        {
            return View();
        }

        [ActionName("PasswordChangeCover")]
        public IActionResult PasswordChangeCover()
        {
            return View();
        }


        [ActionName("PasswordResetCover")]
        public IActionResult PasswordResetCover()
        {
            return View();
        }

        [ActionName("LockScreenBasic")]
        public IActionResult LockScreenBasic()
        {
            return View();
        }

        [ActionName("LockScreenCover")]
        public IActionResult LockScreenCover()
        {
            return View();
        }


        [ActionName("SuccessMessageBasic")]
        public IActionResult SuccessMessageBasic()
        {
            return View();
        }

        [ActionName("SuccessMessageCover")]
        public IActionResult SuccessMessageCover()
        {
            return View();
        }

        [ActionName("TwoStepVerificationBasic")]
        public IActionResult TwoStepVerificationBasic()
        {
            return View();
        }

        [ActionName("TwoStepVerificationCover")]
        public IActionResult TwoStepVerificationCover()
        {
            return View();
        }

        [ActionName("Errors404Basic")]
        public IActionResult Errors404Basic()
        {
            return View();
        }

        [ActionName("Errors404Cover")]
        public IActionResult Errors404Cover()
        {
            return View();
        }

        [ActionName("Errors404Alt")]
        public IActionResult Errors404Alt()
        {
            return View();
        }

        [ActionName("Errors500")]
        public IActionResult Errors500()
        {
            return View();
        }
    }

    internal class InterViewListStudent
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public int Interview_id { get; set; }
        public int Company_id { get; set; }
        public string Company_name { get; set; }
        public int Company_hiring_id { get; set; }
        public string Program_name { get; set; }
    }

    internal class Student
    {
        public int Id { get; set; }
        public string Enro { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string CGPA { get; set; }
        public string Marks9 { get; set; }
        public string Marks10 { get; set; }
        public string Marks11 { get; set; }
        public string Marks12 { get; set; }
        public string IsApproved { get; set; }
    }
}