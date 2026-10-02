using System;

namespace VehicleParkingSystem.Services
{
    public class VehicleTypeValidator
    {
        public bool IsValid(string vehicleType)
        {
            return vehicleType == "Car" ||
                   vehicleType == "Bike" ||
                   vehicleType == "Truck";
        }
    }
}