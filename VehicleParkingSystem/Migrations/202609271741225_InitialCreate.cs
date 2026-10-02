namespace VehicleParkingSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParkingRecord",
                c => new
                    {
                        ParkingRecordId = c.Int(nullable: false, identity: true),
                        VehicleNumber = c.String(nullable: false, maxLength: 20),
                        UserId = c.Int(nullable: false),
                        ParkingSpaceId = c.Int(nullable: false),
                        EntryTime = c.DateTime(nullable: false),
                        ExitTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.ParkingRecordId)
                .ForeignKey("dbo.ParkingSpace", t => t.ParkingSpaceId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ParkingSpaceId);
            
            CreateTable(
                "dbo.ParkingSpace",
                c => new
                    {
                        ParkingSpaceId = c.Int(nullable: false, identity: true),
                        SpaceNumber = c.String(nullable: false, maxLength: 20),
                        IsOccupied = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ParkingSpaceId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 150),
                        PasswordHash = c.String(nullable: false),
                        Role = c.String(nullable: false, maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ParkingRecord", "UserId", "dbo.User");
            DropForeignKey("dbo.ParkingRecord", "ParkingSpaceId", "dbo.ParkingSpace");
            DropIndex("dbo.ParkingRecord", new[] { "ParkingSpaceId" });
            DropIndex("dbo.ParkingRecord", new[] { "UserId" });
            DropTable("dbo.User");
            DropTable("dbo.ParkingSpace");
            DropTable("dbo.ParkingRecord");
        }
    }
}
