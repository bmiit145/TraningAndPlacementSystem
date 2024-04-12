
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Collections.Generic;
using TPS.Controllers;
using System.Data.SqlClient;

namespace TPS.Models
{
	public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Range(0, 255)]
        public byte Role { get; set; }

        public string? RememberToken { get; set; }

        public User()
        {
            Username = string.Empty;
            Password = string.Empty;
        }
        public User(int id, string username, string password, byte role, string? rememberToken)
        {
            Id = id;
            Username = username;
            Password = password;
            Role = role;
            RememberToken = rememberToken;
        }

        public User(int id, string username, string password, byte role)
        {
            Id = id;
            Username = username;
            Password = password;
            Role = role;
        }

        public static List<User> GetUsers()
        {
            DbController db = new DbController();
            db.open();
            string query = "SELECT * FROM users";
            SqlCommand command = new SqlCommand(query, db.conn);
            SqlDataReader reader = command.ExecuteReader();
            List<User> users = new List<User>();
            while (reader.Read())
            {
                User user = new User( reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetByte(3));
                users.Add(user);
                }
            db.close();

            return users;

        }
    }
}

