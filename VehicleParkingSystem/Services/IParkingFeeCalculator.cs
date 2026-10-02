namespace VehicleParkingSystem.Services
{
    public interface IParkingFeeCalculator
    {
        decimal CalculateFee(
            string vehicleType,
            System.DateTime entryTime,
            System.DateTime exitTime);
    }
}