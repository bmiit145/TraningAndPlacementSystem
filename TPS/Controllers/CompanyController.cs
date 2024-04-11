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
    public class CompanyController : Controller
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
            
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            return View("AddCompany");
        }

        public ActionResult addCompanySubmit(string name,string i_type,string email,string description)
        {
            
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            
            if (name == null || i_type == null || email == null || description == null)
            {
                ViewBag.Message = "All Fields are Required";
                return View("AddCompany");
            }
            
            CompanyValidation companyValidation = new CompanyValidation();
            if (!companyValidation.ValidateName(name) || name.Length < 3 || name.Length > 50)
            {
                ViewBag.Message = "Invalid Name";
                return View("AddCompany");
            }
            if (!companyValidation.ValidateEmail(email) || email.Length < 3 || email.Length > 50)
            {
                ViewBag.Message = "Invalid Email";
                return View("AddCompany");
            }
            if (!companyValidation.ValidateCompanySector(i_type) || i_type.Length < 3 || i_type.Length > 50)
            {
                ViewBag.Message = "Invalid Industry Type";
                return View("AddCompany");
            }
            if (!companyValidation.ValidateCompanyDescription(description) || description.Length < 10 || description.Length > 100)
            {
                ViewBag.Message = "Invalid Description";
                return View("AddCompany");
            }
            db.open();
            
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
            
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            
            SqlCommand cmd = new SqlCommand("DELETE FROM CompanyProfile WHERE company_id=@id", db.conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            ViewBag.Message = "Company Deleted Successfully";
            return RedirectToAction("Index");
        }

        [ActionName("editCompany")]
        public ActionResult editCompany(int id)
        {
            
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            
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

        [ActionName("updateCompanySubmit")]
        public ActionResult updateCompanySubmit(string id,string name,string i_type,string email,string description)
        {
            
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "1")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
            db.open();
            
            SqlCommand cmd = new SqlCommand("UPDATE CompanyProfile SET company_name=@name,industry_type=@i_type,email=@email,company_description=@description WHERE company_id=@id", db.conn);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@i_type", i_type);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.ExecuteNonQuery();
            ViewBag.Message = "Company Updated Successfully";
            return RedirectToAction("Index");
        }

        // action that will return all the companies and with number of interviews scheduled for each company
        [ActionName("AllCompaniesStudent")]
        public IActionResult AllCompaniesWith()
        {
            
            if (HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role") != "0")
            {
                return RedirectToAction("SignIn", "Authentication");
            }
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
            reader.Close();
            ViewBag.companies = companies;
            return View("AllCompaniesStudent");
        }
    }

}

