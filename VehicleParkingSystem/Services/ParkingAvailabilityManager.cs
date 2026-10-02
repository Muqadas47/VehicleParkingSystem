using System.Linq;
using VehicleParkingSystem.Data;

namespace VehicleParkingSystem.Services
{
    public class ParkingAvailabilityManager
    {
        private static ParkingAvailabilityManager instance;

        private static readonly object lockObject =
            new object();

        private int totalSpaces;
        private int availableSpaces;


        private ParkingAvailabilityManager()
        {
        }


        public static ParkingAvailabilityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance =
                                new ParkingAvailabilityManager();
                        }
                    }
                }

                return instance;
            }
        }


        public int TotalSpaces
        {
            get { return totalSpaces; }
        }


        public int AvailableSpaces
        {
            get { return availableSpaces; }
        }


        public int OccupiedSpaces
        {
            get
            {
                return totalSpaces - availableSpaces;
            }
        }


        
        public void SyncFromDatabase()
        {
            using (var db =
                new ApplicationDbContext())
            {
                totalSpaces =
                    db.ParkingSpaces.Count();

                availableSpaces =
                    db.ParkingSpaces.Count(
                        x => !x.IsOccupied
                    );
            }
        }
    }
}