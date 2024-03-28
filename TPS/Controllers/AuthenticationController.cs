using Microsoft.AspNetCore.Http;
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


        public ActionResult signinSubmit(string username, string password , bool rememberMe = false)
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
        public ActionResult registerSubmit(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter a username and password.";
                return View("SignUp");
            }

            db.open();
            SqlCommand cmd = new SqlCommand("insert into users (username , password , role) values (@username , @password , @role)", db.conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@role", 0);
            cmd.ExecuteNonQuery();
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

        public IActionResult ResetLinkSend(string username){
            if (string.IsNullOrEmpty(username))
            {
                ViewBag.Error = "Please enter a username.";
                return View("PasswordReset");
            }


            db.close();
            db.open();
            SqlCommand cmd = new SqlCommand("select id, username, role from users where username = @username", db.conn);
            cmd.Parameters.AddWithValue("@username", username);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                // get role if role = 1 then error as No admin password reset
                while (dr.Read())
                {
                    if (dr["role"].ToString() == "1")
                    {
                        ViewBag.Error = "Admin password reset is not allowed.";
                        return View("PasswordReset");
                    }
                    String id = dr["id"].ToString();
                
                // get email from studentProfile table
                db.close();
                db.open();
                cmd = new SqlCommand("select email from StudentProfile where id = @id", db.conn);
                cmd.Parameters.AddWithValue("@id", id);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string email = dr["email"].ToString();
                        string username1 = dr["username"].ToString();
                        // genrate token of 32 character
                        string token = Guid.NewGuid().ToString().Replace("-", "");
                        string resetUrl = $"{this.Request.Scheme}://{this.Request.Host}/Authentication/PasswordChangeRequest?token={token}&username={username1}";
                        // save token in database
                        db.close();
                        db.open();
                        SqlCommand cmd1 = new SqlCommand("insert into PasswordResetToken (username , token) values (@username , @token)", db.conn);
                        cmd1.Parameters.AddWithValue("@username", username1);
                        cmd1.Parameters.AddWithValue("@token", token);
                        cmd1.ExecuteNonQuery();
                        db.close();
                        SendResetPasswordEmail(email, resetUrl);
                    }
                
                ViewBag.Success = "Reset link has been sent to your email.";
                return View("PasswordReset");
                }
                }
                
            }
            else
            {
                ViewBag.Error = "Invalid username.";
                return View("PasswordReset");
            }

            ViewBag.Error = "Something Wents Wrong!";
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

        [ActionName("UpdatePassword")]
        public IActionResult UpdatePassword(string username, string password , string con_password)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(con_password))
            {
                ViewBag.Error = "Please enter a password and confirm password.";
                return View("PasswordChange");
            }

            if(String.IsNullOrEmpty(username))
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
            return RedirectToAction("SignIn" , "Authentication");
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
            return View();
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
}
