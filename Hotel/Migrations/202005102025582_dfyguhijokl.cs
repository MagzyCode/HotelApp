namespace Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dfyguhijokl : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Rooms", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rooms", "Status", c => c.String());
        }
    }
}
