using System;
using System.Linq;
using VehicleParkingSystem.Data;
using VehicleParkingSystem.Models;

namespace VehicleParkingSystem.Services
{
    public class PaymentService : IPaymentService
    {
        public ParkingRecord ProcessPayment(
            string ticketNumber,
            string paymentMethod)
        {
            using (var db = new ApplicationDbContext())
            {
                var parkingRecord = db.ParkingRecords
                    .FirstOrDefault(x =>
                        x.TicketNumber == ticketNumber);

                if (parkingRecord == null)
                {
                    throw new InvalidOperationException(
                        "Parking record was not found.");
                }

                if (parkingRecord.ParkingFee == null)
                {
                    throw new InvalidOperationException(
                        "Parking fee has not been calculated.");
                }
                if (paymentMethod != "Cash" &&
                    paymentMethod != "Card" &&
                    paymentMethod != "JazzCash")
                {
                    throw new ArgumentException(
                        "Payment method must be Cash, Card, or JazzCash.");
                }

                if (parkingRecord.PaymentStatus == "Paid")
                {
                    throw new InvalidOperationException(
                        "Payment has already been completed.");
                }

                parkingRecord.PaymentMethod = paymentMethod;
                parkingRecord.PaymentStatus = "Paid";

                db.SaveChanges();

                return parkingRecord;
            }
        }
    }
}