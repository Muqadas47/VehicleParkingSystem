using System;

namespace VehicleParkingSystem.Services
{
    public class ParkingFeeCalculator : IParkingFeeCalculator
    {
        public decimal CalculateFee(
            string vehicleType,
            DateTime entryTime,
            DateTime exitTime)
        {
            var totalHours = Math.Ceiling(
                (exitTime - entryTime).TotalHours);

            if (totalHours < 1)
            {
                totalHours = 1;
            }

            var hourlyRate =
                GetHourlyRate(vehicleType);

            return (decimal)totalHours * hourlyRate;
        }

        private decimal GetHourlyRate(string vehicleType)
        {
            switch (vehicleType)
            {
                case "Car":
                    return 100;

                case "Bike":
                    return 50;

                case "Truck":
                    return 150;

                default:
                    throw new ArgumentException(
                        "Invalid vehicle type.");
            }
        }
    }
}