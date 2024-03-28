
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace TPS.Models
{
	public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        [Range(0, 255)]
        public byte Role { get; set; }

        public string? RememberToken { get; set; }
    }
}

