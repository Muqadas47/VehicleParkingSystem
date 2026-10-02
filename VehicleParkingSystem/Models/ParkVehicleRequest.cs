using System.ComponentModel.DataAnnotations;

namespace VehicleParkingSystem.Models
{
    public class ParkVehicleRequest
    {
        [Required]
        [MaxLength(20)]
        public string VehicleNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string VehicleType { get; set; }
    }
}