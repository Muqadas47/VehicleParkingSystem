using System.ComponentModel.DataAnnotations;

namespace VehicleParkingSystem.Models
{
    public class VehicleExitRequest
    {
        [Required]
        [MaxLength(20)]
        public string TicketNumber { get; set; }
    }
}