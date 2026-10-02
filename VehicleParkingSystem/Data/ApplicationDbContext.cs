using System.Data.Entity;
using VehicleParkingSystem.Models;

namespace VehicleParkingSystem.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext()
			: base("name=VehicleParkingConnection")
		{
		}

		public DbSet<User> Users { get; set; }

		public DbSet<ParkingSpace> ParkingSpaces { get; set; }

		public DbSet<ParkingRecord> ParkingRecords { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.ToTable("User");

			modelBuilder.Entity<ParkingSpace>()
				.ToTable("ParkingSpace");

			modelBuilder.Entity<ParkingRecord>()
				.ToTable("ParkingRecord");

			base.OnModelCreating(modelBuilder);
		}
	}
}