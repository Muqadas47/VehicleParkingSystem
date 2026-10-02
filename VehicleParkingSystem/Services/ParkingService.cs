using System;
using System.Linq;
using VehicleParkingSystem.Data;
using VehicleParkingSystem.Models;

namespace VehicleParkingSystem.Services
{
    public class ParkingService
    {
        private readonly IParkingFeeCalculator feeCalculator;

        public ParkingService(IParkingFeeCalculator feeCalculator)
        {
            this.feeCalculator = feeCalculator;
        }

        public ParkingRecord ParkVehicle(
            string vehicleNumber,
            string vehicleType,
            int userId)
        {
            using (var db = new ApplicationDbContext())

            {
                var vehicleTypeValidator =
    new VehicleTypeValidator();
                // if ABC-123 is already inside the parking and has no ExitTime, the system won't allow ABC-123 to be parked again.
                if (!vehicleTypeValidator.IsValid(vehicleType))
                {
                    throw new ArgumentException(
                        "Vehicle type must be Car, Bike, or Truck.");
                }

                vehicleNumber = vehicleNumber.Trim();

                var alreadyParked =
                    db.ParkingRecords.Any(x =>
                        x.VehicleNumber == vehicleNumber &&
                        x.ExitTime == null);

                if (alreadyParked)
                {
                    throw new InvalidOperationException(
                        "This vehicle is already parked.");
                }

                var parkingSpace = db.ParkingSpaces
                    .FirstOrDefault(x => !x.IsOccupied);

                if (parkingSpace == null)
                {
                    throw new InvalidOperationException(
                        "No parking space is available.");
                }

                var ticketNumber =
                    "TKT-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var parkingRecord = new ParkingRecord
                {
                    VehicleNumber = vehicleNumber,
                    VehicleType = vehicleType,
                    TicketNumber = ticketNumber,
                    UserId = userId,
                    ParkingSpaceId = parkingSpace.ParkingSpaceId,
                    EntryTime = DateTime.Now
                };

                parkingSpace.IsOccupied = true;

                db.ParkingRecords.Add(parkingRecord);
                db.SaveChanges();

                ParkingAvailabilityManager.Instance
                    .SyncFromDatabase();

                return parkingRecord;
            }
        }

        public ParkingRecord ExitVehicle(string ticketNumber)
        {
            using (var db = new ApplicationDbContext())
            {
                var parkingRecord = db.ParkingRecords
                    .FirstOrDefault(x =>
                        x.TicketNumber == ticketNumber &&
                        x.ExitTime == null);

                if (parkingRecord == null)
                {
                    throw new InvalidOperationException(
                        "Active parking record was not found.");
                }

                var exitTime = DateTime.Now;

                var parkingFee =
     feeCalculator.CalculateFee(
         parkingRecord.VehicleType,
         parkingRecord.EntryTime,
         exitTime);

                parkingRecord.ExitTime = exitTime;
                parkingRecord.ParkingFee = parkingFee;

                var parkingSpace = db.ParkingSpaces
                    .FirstOrDefault(x =>
                        x.ParkingSpaceId ==
                        parkingRecord.ParkingSpaceId);

                if (parkingSpace != null)
                {
                    parkingSpace.IsOccupied = false;
                }

                db.SaveChanges();

                ParkingAvailabilityManager.Instance
                    .SyncFromDatabase();

                return parkingRecord;
            }
        }
    }
}