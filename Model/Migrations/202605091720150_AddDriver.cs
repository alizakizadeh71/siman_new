namespace Models.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddDriver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FactorCements", "DriverName", c => c.String());
            AddColumn("dbo.FactorCements", "DriverLastName", c => c.String());
            AddColumn("dbo.FactorCements", "DriverMobile", c => c.String(maxLength: 11));
            AddColumn("dbo.FactorCements", "DriverLicensePlate", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.FactorCements", "DriverLicensePlate");
            DropColumn("dbo.FactorCements", "DriverMobile");
            DropColumn("dbo.FactorCements", "DriverLastName");
            DropColumn("dbo.FactorCements", "DriverName");
        }
    }
}
