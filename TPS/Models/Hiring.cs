
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using SQLitePCL;
using TPS.Controllers;

namespace TPS.Models
{
	public class Hiring
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Program_name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Range(0, 255)]
        public required DateTime Start_date { get; set; }

        public required DateTime End_date { get; set; }

        public int Course_id { get; set; }

        public string? Course_name { get; set; }

        public Hiring()
        {
            Program_name = "";
            Description = "";
            Start_date = DateTime.Now;
            End_date = DateTime.Now;
            Course_id = 0;
            Course_name = "";

        }
    }
}

