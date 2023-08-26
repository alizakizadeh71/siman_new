namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addtablenews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DestinationManagements",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FinancialManagementId = c.Guid(nullable: false),
                        ProvinceId = c.Guid(),
                        CityId = c.Guid(nullable: false),
                        DestinationAmountPaid = c.Long(nullable: false),
                        IsActived = c.Boolean(nullable: false),
                        IsVerified = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsSystem = c.Boolean(nullable: false),
                        InsertDateTime = c.DateTime(nullable: false),
                        UpdateDateTime = c.DateTime(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.FinancialManagements", t => t.FinancialManagementId, cascadeDelete: true)
                .ForeignKey("dbo.Provinces", t => t.ProvinceId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.FinancialManagementId)
                .Index(t => t.ProvinceId)
                .Index(t => t.CityId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.newswebs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        newsText = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActived = c.Boolean(nullable: false),
                        IsVerified = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsSystem = c.Boolean(nullable: false),
                        InsertDateTime = c.DateTime(nullable: false),
                        UpdateDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.FactorCements", "DestinationAmountPaid", c => c.Long());
            AddColumn("dbo.FactorCements", "MahalTahvil", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DestinationManagements", "User_Id", "dbo.Users");
            DropForeignKey("dbo.DestinationManagements", "ProvinceId", "dbo.Provinces");
            DropForeignKey("dbo.DestinationManagements", "FinancialManagementId", "dbo.FinancialManagements");
            DropForeignKey("dbo.DestinationManagements", "CityId", "dbo.Cities");
            DropIndex("dbo.DestinationManagements", new[] { "User_Id" });
            DropIndex("dbo.DestinationManagements", new[] { "CityId" });
            DropIndex("dbo.DestinationManagements", new[] { "ProvinceId" });
            DropIndex("dbo.DestinationManagements", new[] { "FinancialManagementId" });
            DropColumn("dbo.FactorCements", "MahalTahvil");
            DropColumn("dbo.FactorCements", "DestinationAmountPaid");
            DropTable("dbo.newswebs");
            DropTable("dbo.DestinationManagements");
        }
    }
}
