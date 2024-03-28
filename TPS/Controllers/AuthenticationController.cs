using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [ActionName("PasswordResetBasic")]
        public IActionResult PasswordResetBasic()
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

        [ActionName("Logout")]
        public IActionResult Logout()
        {
            // do the logout process here
            ISession session = HttpContext.Session;
            session.Clear();
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
