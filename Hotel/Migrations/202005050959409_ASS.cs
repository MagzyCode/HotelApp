namespace Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ASS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Fulfilling", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Fulfilling");
        }
    }
}
