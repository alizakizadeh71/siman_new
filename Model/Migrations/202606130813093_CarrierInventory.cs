namespace Models.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CarrierInventory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarrierInventory",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    ProductNameId = c.Guid(nullable: false),
                    ProductTypeId = c.Guid(nullable: false),
                    PackageTypeId = c.Guid(nullable: false),
                    FactoryNameId = c.Guid(nullable: false),
                    CarrierId = c.Guid(nullable: false),
                    InventoryTonnage = c.Double(nullable: false),
                    IsDefaultCarrier = c.Boolean(nullable: false),
                    IsActived = c.Boolean(nullable: false),
                    IsVerified = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    IsSystem = c.Boolean(nullable: false),
                    InsertDateTime = c.DateTime(nullable: false),
                    UpdateDateTime = c.DateTime(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CarrierId, cascadeDelete: false)
                .ForeignKey("dbo.FactoryNames", t => t.FactoryNameId, cascadeDelete: false)
                .ForeignKey("dbo.PackageTypes", t => t.PackageTypeId, cascadeDelete: false)
                .ForeignKey("dbo.ProductNames", t => t.ProductNameId, cascadeDelete: false)
                .ForeignKey("dbo.ProductTypes", t => t.ProductTypeId, cascadeDelete: false)
                .Index(t => t.ProductNameId)
                .Index(t => t.ProductTypeId)
                .Index(t => t.PackageTypeId)
                .Index(t => t.FactoryNameId)
                .Index(t => t.CarrierId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.CarrierInventory", "ProductTypeId", "dbo.ProductTypes");
            DropForeignKey("dbo.CarrierInventory", "ProductNameId", "dbo.ProductNames");
            DropForeignKey("dbo.CarrierInventory", "PackageTypeId", "dbo.PackageTypes");
            DropForeignKey("dbo.CarrierInventory", "FactoryNameId", "dbo.FactoryNames");
            DropForeignKey("dbo.CarrierInventory", "CarrierId", "dbo.Users");
            DropIndex("dbo.CarrierInventory", new[] { "CarrierId" });
            DropIndex("dbo.CarrierInventory", new[] { "FactoryNameId" });
            DropIndex("dbo.CarrierInventory", new[] { "PackageTypeId" });
            DropIndex("dbo.CarrierInventory", new[] { "ProductTypeId" });
            DropIndex("dbo.CarrierInventory", new[] { "ProductNameId" });
            DropTable("dbo.CarrierInventory");
        }
    }
}