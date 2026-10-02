namespace VehicleParkingSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
	using VehicleParkingSystem.Models;
	using VehicleParkingSystem.Services;

    internal sealed class Configuration : DbMigrationsConfiguration<VehicleParkingSystem.Data.ApplicationDbContext>
    {
		protected override void Seed(VehicleParkingSystem.Data.ApplicationDbContext context)
		{
			// ---------------- USERS ----------------

			if (!context.Users.Any())
			{
				var passwordHasher = new PasswordHasher();

				context.Users.Add(new User
				{
					FullName = "Parking Admin",
					Email = "admin@parking.com",
					PasswordHash = passwordHasher.HashPassword("Admin@123"),
					Role = "Admin",
					IsActive = true,
					CreatedAt = DateTime.Now
				});

				context.Users.Add(new User
				{
					FullName = "Test User",
					Email = "user@parking.com",
					PasswordHash = passwordHasher.HashPassword("User@123"),
					Role = "User",
					IsActive = true,
					CreatedAt = DateTime.Now
				});
			}


			

			if (!context.ParkingSpaces.Any())
			{
				context.ParkingSpaces.Add(new ParkingSpace
				{
					SpaceNumber = "A-01",
					IsOccupied = false
				});

				context.ParkingSpaces.Add(new ParkingSpace
				{
					SpaceNumber = "A-02",
					IsOccupied = false
				});

				context.ParkingSpaces.Add(new ParkingSpace
				{
					SpaceNumber = "A-03",
					IsOccupied = false
				});

				context.ParkingSpaces.Add(new ParkingSpace
				{
					SpaceNumber = "A-04",
					IsOccupied = false
				});

				context.ParkingSpaces.Add(new ParkingSpace
				{
					SpaceNumber = "A-05",
					IsOccupied = false
				});
			}


			context.SaveChanges();
		}
	}
}
