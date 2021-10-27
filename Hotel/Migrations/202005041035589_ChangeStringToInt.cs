namespace Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStringToInt : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.RoomOrders");
            AlterColumn("dbo.RoomOrders", "RoomOrderNumber", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.RoomOrders", "RoomOrderNumber");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.RoomOrders");
            AlterColumn("dbo.RoomOrders", "RoomOrderNumber", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.RoomOrders", "RoomOrderNumber");
        }
    }
}
