namespace Models.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddIsManualPreInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FactorCements", "IsManualPreInvoice", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.FactorCements", "IsManualPreInvoice");
        }
    }
}
