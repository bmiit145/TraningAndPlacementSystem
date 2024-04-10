using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPS.Controllers
{
    public class DbController : Controller
    {
       public SqlConnection conn;
        String connectionString = "Server=tcp:tpsbmiit.database.windows.net,1433;Initial Catalog=tps;Persist Security Info=False;User ID=priyank;Password=Spirit@8414;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public DbController()
        {
            conn = new SqlConnection(connectionString);
        }

        public void open()
        {
            conn.Open();
        }

        public void close()
        {
            conn.Close();
        }

        public void InsertInitialAdminData(string username, string password)
        {
            try
            {
                open();

                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM users WHERE Username = @Username", conn);
                checkCmd.Parameters.AddWithValue("@Username", username);
                int existingUserCount = (int)checkCmd.ExecuteScalar();

                // If the username doesn't exist, insert new admin
                if (existingUserCount == 0)
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    // Assuming you have a table named 'Users' with columns 'Username' and 'Password'
                    cmd.CommandText = "INSERT INTO users (Username, Password , role) VALUES (@Username, @Password , 1)";
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                close();
            }
        }
        }
}

