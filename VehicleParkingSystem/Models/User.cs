using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleParkingSystem.Models
{
	public class User
	{
		[Key]
		public int UserId { get; set; }

		[Required]
		[MaxLength(100)]
		public string FullName { get; set; }

		[Required]
		[MaxLength(150)]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		[Required]
		[MaxLength(20)]
		public string Role { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }
	}
}