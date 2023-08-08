namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edittablenews : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.newswebs", "StartDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.newswebs", "EndDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.newswebs", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.newswebs", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
