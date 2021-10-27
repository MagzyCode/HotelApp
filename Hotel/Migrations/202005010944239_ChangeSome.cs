namespace Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSome : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ServiceOrders", "ClientId");
            DropColumn("dbo.ServiceOrders", "ServiceIdInOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceOrders", "ServiceIdInOrder", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceOrders", "ClientId", c => c.String());
        }
    }
}
