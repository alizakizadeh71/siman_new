namespace Models.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddManualBuyerName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FactorCements", "ManualBuyerName", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.FactorCements", "ManualBuyerName");
        }
    }
}
