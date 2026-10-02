using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace VehicleParkingSystem.Models
{
	public class ParkingSpace
	{
		[Key]
		public int ParkingSpaceId { get; set; }

		[Required]
		[MaxLength(20)]
		public string SpaceNumber { get; set; }

		public bool IsOccupied { get; set; }
	}
}