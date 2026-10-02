using System;
using System.Linq;
using System.Web.Http;
using VehicleParkingSystem.Models;
using VehicleParkingSystem.Services;

namespace VehicleParkingSystem.Controllers
{
    [RoutePrefix("api/parking")]
    public class ParkingController : ApiController
    {
        [HttpGet]
        [Route("availability")]
        [Filters.JwtAuthorize]
        public IHttpActionResult GetAvailability()
        {
            var parkingManager =
                ParkingAvailabilityManager.Instance;

            parkingManager.SyncFromDatabase();

            return Ok(new
            {
                totalSpaces =
                    parkingManager.TotalSpaces,

                occupiedSpaces =
                    parkingManager.OccupiedSpaces,

                availableSpaces =
                    parkingManager.AvailableSpaces
            });
        }

        [HttpPost]
        [Route("park")]
        [Filters.JwtAuthorize]
        public IHttpActionResult ParkVehicle(
            ParkVehicleRequest model)
        {
            if (model == null)
            {
                return BadRequest(
                    "Vehicle details are required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

     

            try
            {
                //hardcodessd user=1
                var userIdClaim =
     User.Identity as System.Security.Claims.ClaimsIdentity;

                var userIdValue =
                    userIdClaim.FindFirst(
                        System.Security.Claims.ClaimTypes.NameIdentifier);

                if (userIdValue == null)
                {
                    return Unauthorized();
                }

                var userId = int.Parse(userIdValue.Value);

                var parkingService =
    new ParkingService(
        new ParkingFeeCalculator());

                var record =
                    parkingService.ParkVehicle(
                        model.VehicleNumber,
                        model.VehicleType,
                        userId);

                return Ok(new
                {
                    message = "Vehicle parked successfully.",
                    ticketNumber = record.TicketNumber,
                    vehicleNumber = record.VehicleNumber,
                    vehicleType = record.VehicleType,
                    entryTime = record.EntryTime,
                    parkingSpaceId = record.ParkingSpaceId
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("exit")]
        [Filters.JwtAuthorize]
        public IHttpActionResult ExitVehicle(
            VehicleExitRequest model)
        {
            if (model == null)
            {
                return BadRequest(
                    "Ticket number is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var parkingService =
    new ParkingService(
        new ParkingFeeCalculator());

                var record =
                    parkingService.ExitVehicle(
                        model.TicketNumber);

                return Ok(new
                {
                    message = "Vehicle exit completed.",
                    ticketNumber = record.TicketNumber,
                    vehicleNumber = record.VehicleNumber,
                    vehicleType = record.VehicleType,
                    entryTime = record.EntryTime,
                    exitTime = record.ExitTime,
                    parkingFee = record.ParkingFee
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        [HttpPost]
        [Route("payment")]
        [Filters.JwtAuthorize]
        public IHttpActionResult ProcessPayment(
    PaymentRequest model)
        {
            if (model == null)
            {
                return BadRequest(
                    "Payment details are required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                IPaymentService paymentService =
    new PaymentService();

                var record =
                    paymentService.ProcessPayment(
                        model.TicketNumber,
                        model.PaymentMethod);

                return Ok(new
                {
                    message = "Payment completed successfully.",
                    ticketNumber = record.TicketNumber,
                    parkingFee = record.ParkingFee,
                    paymentMethod = record.PaymentMethod,
                    paymentStatus = record.PaymentStatus
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        [HttpGet]
        [Route("occupied")]
        [Filters.JwtAuthorize]
        public IHttpActionResult GetOccupiedVehicles()
        {
            try
            {
                using (var db = new Data.ApplicationDbContext())
                {
                    var vehicles = db.ParkingRecords
                        .Where(x => x.ExitTime == null)
                        .OrderBy(x => x.EntryTime)
                        .Select(x => new
                        {
                            vehicleNumber = x.VehicleNumber,
                            vehicleType = x.VehicleType,
                            ticketNumber = x.TicketNumber,
                            parkingSpaceId = x.ParkingSpaceId,
                            entryTime = x.EntryTime
                        })
                        .ToList();

                    return Ok(vehicles);
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }

}