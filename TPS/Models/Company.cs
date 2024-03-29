
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace TPS.Models
{
	public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string IndustryType { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Description { get; set; }

        public string? ImgUrl { get; set; }
    }
}

