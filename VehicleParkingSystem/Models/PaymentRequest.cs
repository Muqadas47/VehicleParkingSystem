using System.ComponentModel.DataAnnotations;

namespace VehicleParkingSystem.Models
{
    public class PaymentRequest
    {
        [Required]
        [MaxLength(20)]
        public string TicketNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string PaymentMethod { get; set; }
    }
}