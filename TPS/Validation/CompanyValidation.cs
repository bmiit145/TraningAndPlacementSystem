// write a list of regex patterns to validate company names
// each pattern should be a string

using System.Text.RegularExpressions;

namespace TPS.Validation
{
    public class CompanyValidation
    {
        // name only accepts only characters and spaces length 3-50
        String namePattern = @"^[a-zA-Z\s]{3,50}$";
        String emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        String companySectorPattern = @"^[a-zA-Z\s]{3,50}$";
        String companyDescriptionPattern = @"^[a-zA-Z\s]{3,50}$";

        public CompanyValidation()
        {
        }

        public bool ValidateName(string name)
        {
            return Regex.IsMatch(name, namePattern);
        }

        public bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, emailPattern);
        }

        public bool ValidateCompanySector(string companySector)
        {
            return Regex.IsMatch(companySector, companySectorPattern);
        }

        public bool ValidateCompanyDescription(string companyDescription)
        {
            return Regex.IsMatch(companyDescription, companyDescriptionPattern);
        }

    }
}
        
