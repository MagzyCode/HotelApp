namespace Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SecondName = c.String(),
                        Patronymic = c.String(),
                        DayOfBorn = c.DateTime(nullable: false),
                        Gender = c.String(),
                        PersonalMail = c.String(),
                        Passport = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.RoomOrders",
                c => new
                    {
                        RoomOrderNumber = c.String(nullable: false, maxLength: 128),
                        ClientId = c.Int(nullable: false),
                        RoomNumberInOrder = c.Int(nullable: false),
                        SettlementDate = c.DateTime(nullable: false),
                        DepartureDate = c.DateTime(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        TotalPrice = c.Int(nullable: false),
                        Room_RoomNumber = c.Int(),
                    })
                .PrimaryKey(t => t.RoomOrderNumber)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Rooms", t => t.Room_RoomNumber)
                .Index(t => t.ClientId)
                .Index(t => t.EmployeeId)
                .Index(t => t.Room_RoomNumber);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SecondName = c.String(),
                        Patronymic = c.String(),
                        DayOfBorn = c.DateTime(nullable: false),
                        Gender = c.String(),
                        PersonalMail = c.String(),
                        Password = c.String(),
                        Post = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.ServiceOrders",
                c => new
                    {
                        ServiceOrderId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        ClientId = c.String(),
                        ServiceIdInOrder = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Client_ClientId = c.Int(),
                        Service_ServiceId = c.Int(),
                    })
                .PrimaryKey(t => t.ServiceOrderId)
                .ForeignKey("dbo.Clients", t => t.Client_ClientId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.Service_ServiceId)
                .Index(t => t.EmployeeId)
                .Index(t => t.Client_ClientId)
                .Index(t => t.Service_ServiceId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        ServiceCost = c.Int(nullable: false),
                        Explanation = c.String(),
                    })
                .PrimaryKey(t => t.ServiceId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomNumber = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        RoomType_Type = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RoomNumber)
                .ForeignKey("dbo.RoomTypes", t => t.RoomType_Type)
                .Index(t => t.RoomType_Type);
            
            CreateTable(
                "dbo.RoomTypes",
                c => new
                    {
                        Type = c.String(nullable: false, maxLength: 128),
                        Cost = c.Int(nullable: false),
                        Description = c.String(),
                        Area = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Type);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rooms", "RoomType_Type", "dbo.RoomTypes");
            DropForeignKey("dbo.RoomOrders", "Room_RoomNumber", "dbo.Rooms");
            DropForeignKey("dbo.ServiceOrders", "Service_ServiceId", "dbo.Services");
            DropForeignKey("dbo.ServiceOrders", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.ServiceOrders", "Client_ClientId", "dbo.Clients");
            DropForeignKey("dbo.RoomOrders", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.RoomOrders", "ClientId", "dbo.Clients");
            DropIndex("dbo.Rooms", new[] { "RoomType_Type" });
            DropIndex("dbo.ServiceOrders", new[] { "Service_ServiceId" });
            DropIndex("dbo.ServiceOrders", new[] { "Client_ClientId" });
            DropIndex("dbo.ServiceOrders", new[] { "EmployeeId" });
            DropIndex("dbo.RoomOrders", new[] { "Room_RoomNumber" });
            DropIndex("dbo.RoomOrders", new[] { "EmployeeId" });
            DropIndex("dbo.RoomOrders", new[] { "ClientId" });
            DropTable("dbo.RoomTypes");
            DropTable("dbo.Rooms");
            DropTable("dbo.Services");
            DropTable("dbo.ServiceOrders");
            DropTable("dbo.Employees");
            DropTable("dbo.RoomOrders");
            DropTable("dbo.Clients");
        }
    }
}
