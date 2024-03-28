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
            db.InsertInitialAdminData("admin" , "admin");
            return View();
        }


        public ActionResult signinSubmit(string username, string password)
        {

            db.open();
            SqlCommand cmd = new SqlCommand("select username , password , role from users where username = @username AND password = @password ", db.conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows){
                while (dr.Read())
                {
                    // store username and role in session
                    HttpContext.Session.SetString("username", dr["username"].ToString());
                    HttpContext.Session.SetString("role", dr["role"].ToString());

                    // admin role as 1 and student role as 0
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

        [ActionName("LogoutBasic")]
        public IActionResult LogoutBasic()
        {
            return View();
        }

        [ActionName("LogoutCover")]
        public IActionResult LogoutCover()
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
