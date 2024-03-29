using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TPS.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPS.Controllers
{
    public class CompanyController : Controller
    {
        DbController db = new DbController();

        [ActionName("Index")]
        public IActionResult Index()
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            // get all companies from database and send to view
            db.open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM CompanyProfile", db.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Company> companies = new List<Company>();
            while (reader.Read())
            {
                Company company = new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    IndustryType = reader.GetString(2),
                    Email = reader.GetString(3),
                    Description = reader.GetString(4)
                };
                companies.Add(company);
            }
            ViewBag.companies = companies;
            return View("AllCompanies");
        }

        [ActionName("addCompany")]
        public ActionResult addCompany()
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            return View("AddCompany");
        }

        public ActionResult addCompanySubmit(string name,string i_type,string email,string description)
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            //add company to database
            SqlCommand cmd = new SqlCommand("INSERT INTO CompanyProfile(company_name,industry_type,email,company_description) VALUES(@name,@i_type,@email,@description)",db.conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@i_type", i_type);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.ExecuteNonQuery();
            ViewBag.Message = "Company Added Successfully";
            return RedirectToAction("Index");
        }

        public ActionResult deleteCompany(int id)
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            //delete company from database
            SqlCommand cmd = new SqlCommand("DELETE FROM CompanyProfile WHERE company_id=@id", db.conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            ViewBag.Message = "Company Deleted Successfully";
            return View("Index");
        }

        [ActionName("editCompany")]
        public ActionResult editCompany(int id)
        {
            // if session is not available then redirect to signin page
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            //get company from database
            SqlCommand cmd = new SqlCommand("SELECT * FROM CompanyProfile WHERE company_id=@id", db.conn);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
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
            return View("EditCompany");
        }
    }

}

