namespace VehicleParkingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVehicleParkingDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParkingRecord", "VehicleType", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.ParkingRecord", "TicketNumber", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.ParkingRecord", "ParkingFee", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ParkingRecord", "PaymentMethod", c => c.String(maxLength: 20));
            AddColumn("dbo.ParkingRecord", "PaymentStatus", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParkingRecord", "PaymentStatus");
            DropColumn("dbo.ParkingRecord", "PaymentMethod");
            DropColumn("dbo.ParkingRecord", "ParkingFee");
            DropColumn("dbo.ParkingRecord", "TicketNumber");
            DropColumn("dbo.ParkingRecord", "VehicleType");
        }
    }
}
