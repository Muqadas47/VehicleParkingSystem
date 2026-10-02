namespace VehicleParkingSystem.Services
{
    public interface IPaymentService
    {
        Models.ParkingRecord ProcessPayment(
            string ticketNumber,
            string paymentMethod);
    }
}