namespace Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeOfOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomOrders", "TimeOfRoomOrder", c => c.DateTime(nullable: false));
            AddColumn("dbo.ServiceOrders", "TimeOfOrder", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceOrders", "TimeOfOrder");
            DropColumn("dbo.RoomOrders", "TimeOfRoomOrder");
        }
    }
}
