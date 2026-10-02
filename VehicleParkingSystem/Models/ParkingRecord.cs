using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleParkingSystem.Models
{
    public class ParkingRecord
    {
        [Key]
        public int ParkingRecordId { get; set; }

        [Required]
        [MaxLength(20)]
        public string VehicleNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string VehicleType { get; set; }

        [Required]
        [MaxLength(20)]
        public string TicketNumber { get; set; }

        public int UserId { get; set; }

        public int ParkingSpaceId { get; set; }

        public DateTime EntryTime { get; set; }

        public DateTime? ExitTime { get; set; }

        public decimal? ParkingFee { get; set; }

        [MaxLength(20)]
        public string PaymentMethod { get; set; }

        [MaxLength(20)]
        public string PaymentStatus { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ParkingSpaceId")]
        public virtual ParkingSpace ParkingSpace { get; set; }
    }
}