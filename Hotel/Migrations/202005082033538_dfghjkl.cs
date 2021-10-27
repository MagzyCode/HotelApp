namespace Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dfghjkl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomTypes", "Capacity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomTypes", "Capacity");
        }
    }
}
