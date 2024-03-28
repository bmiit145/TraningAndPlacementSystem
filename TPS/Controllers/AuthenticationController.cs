using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPS.Controllers
{
    public class AuthenticationController : Controller
    {
        [ActionName("SignInBasic")]
        public IActionResult SignInBasic()
        {

            DbController db = new DbController();
            db.InsertInitialAdminData("admin" , "admin");
            return View();
        }

        [ActionName("Offline")]
        public ActionResult Offline()
        {
            return View("~/Views/Authentication/offline.cshtml");
        }



        [ActionName("SignInCover")]
        public IActionResult SignInCover()
        {
            return View();
        }

        [ActionName("SignUpBasic")]
        public IActionResult SignUpBasic()
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
