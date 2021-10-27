namespace Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTotalCost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceOrders", "TotalPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceOrders", "TotalPrice");
        }
    }
}
