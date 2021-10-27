namespace Hotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoNotify : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Services", "Explanation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Explanation", c => c.String());
        }
    }
}
