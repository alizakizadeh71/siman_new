namespace Models
{
    public class DatabaseContext : System.Data.Entity.DbContext
    {
        static DatabaseContext()
        {
            //System.Data.Entity.Database.SetInitializer
            //    (new DatabaseContextInitializerBeforeTheFirstRelease());

            System.Data.Entity.Database.SetInitializer
                (new System.Data.Entity.MigrateDatabaseToLatestVersion
                <DatabaseContext, Migrations.Configuration>());
        }

        public DatabaseContext()
        {
           // ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext.CommandTimeout = 120;
        }

        public System.Data.Entity.DbSet<DetailOfFactor> DetailOfFactors { get; set; }
        public System.Data.Entity.DbSet<FactorMessage> FactorMessages { get; set; }
        public System.Data.Entity.DbSet<HeadOfFactor> HeadOfFactors { get; set; }
        public System.Data.Entity.DbSet<Commodity> Commoditys { get; set; }
        public System.Data.Entity.DbSet<CommodityInSubSystem> CommodityInSubSystems { get; set; }
        public System.Data.Entity.DbSet<ServiceTariffInSubSystem> ServiceTariffInSubSystems { get; set; }
        public System.Data.Entity.DbSet<ServiceTariff> ServiceTariffs { get; set; }
        public System.Data.Entity.DbSet<Unit> Units { get; set; }
        public System.Data.Entity.DbSet<BankAccount> BankAccounts { get; set; }
        public System.Data.Entity.DbSet<Certain> Certains { get; set; }
        public System.Data.Entity.DbSet<IncomeRow> IncomeRows { get; set; }
        public System.Data.Entity.DbSet<ExecutableCode> ExecutableCodes { get; set; }
        public System.Data.Entity.DbSet<Bank> Banks { get; set; }
        public System.Data.Entity.DbSet<HeadLine> HeadLines { get; set; }
        public System.Data.Entity.DbSet<SubHeadLine> SubHeadLines { get; set; }
        public System.Data.Entity.DbSet<CurrencyUnit> CurrencyUnits { get; set; }
        public System.Data.Entity.DbSet<CurrencyUnitLog> CurrencyUnitLogs { get; set; }
        public System.Data.Entity.DbSet<AccountNumberManage> AccountNumberManages { get; set; }
        public System.Data.Entity.DbSet<AccountNumberManageLog> AccountNumberManageLogs { get; set; }
        public System.Data.Entity.DbSet<SubSystem> SubSystems { get; set; }
        public System.Data.Entity.DbSet<Province> Provinces { get; set; }
        public System.Data.Entity.DbSet<PaymentHeader> PaymentHeaders { get; set; }
        public System.Data.Entity.DbSet<PaymentDetail> PaymentDetails { get; set; }
        public System.Data.Entity.DbSet<AccountNumber> AccountNumbers { get; set; }
        public System.Data.Entity.DbSet<ErrorLog> ErrorLogs { get; set; }
        public System.Data.Entity.DbSet<Role> Roles { get; set; }
        public System.Data.Entity.DbSet<User> Users { get; set; }
        public System.Data.Entity.DbSet<Request> Requests { get; set; }
        public System.Data.Entity.DbSet<File> Files { get; set; }
        public System.Data.Entity.DbSet<Message> Messages { get; set; }
        public System.Data.Entity.DbSet<ProjectAction> ProjectActions { get; set; }
        public System.Data.Entity.DbSet<AccessType> AccessTypes { get; set; }
        public System.Data.Entity.DbSet<UserLoginLog> UserLoginLogs { get; set; }
        public System.Data.Entity.DbSet<ProductName> ProductNames{ get; set; }
        public System.Data.Entity.DbSet<ProductType> ProductTypes{ get; set; }
        public System.Data.Entity.DbSet<FactorCement> FactorCements { get; set; }
        public System.Data.Entity.DbSet<PackageType> PackageTypes{ get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new DetailOfFactor.Configuration());
            modelBuilder.Configurations.Add(new FactorMessage.Configuration());
            modelBuilder.Configurations.Add(new HeadOfFactor.Configuration());
            modelBuilder.Configurations.Add(new Commodity.Configuration());
            modelBuilder.Configurations.Add(new CommodityInSubSystem.Configuration());
            modelBuilder.Configurations.Add(new ServiceTariffInSubSystem.Configuration());
            modelBuilder.Configurations.Add(new ServiceTariff.Configuration());
            modelBuilder.Configurations.Add(new Unit.Configuration());
            modelBuilder.Configurations.Add(new BankAccount.Configuration());
            modelBuilder.Configurations.Add(new Certain.Configuration());
            modelBuilder.Configurations.Add(new IncomeRow.Configuration());
            modelBuilder.Configurations.Add(new ExecutableCode.Configuration());
            modelBuilder.Configurations.Add(new Bank.Configuration());
            modelBuilder.Configurations.Add(new SubHeadLine.Configuration());
            modelBuilder.Configurations.Add(new HeadLine.Configuration());
            modelBuilder.Configurations.Add(new CurrencyUnit.Configuration());
            modelBuilder.Configurations.Add(new CurrencyUnitLog.Configuration());
            modelBuilder.Configurations.Add(new AccountNumberManage.Configuration());
            modelBuilder.Configurations.Add(new AccountNumberManageLog.Configuration());
            modelBuilder.Configurations.Add(new SubSystem.Configuration());
            modelBuilder.Configurations.Add(new Province.Configuration());
            modelBuilder.Configurations.Add(new PaymentHeader.Configuration());
            modelBuilder.Configurations.Add(new PaymentDetail.Configuration());
            modelBuilder.Configurations.Add(new AccountNumber.Configuration());

            modelBuilder.Configurations.Add(new ErrorLog.Configuration());
            modelBuilder.Configurations.Add(new Role.Configuration());
            modelBuilder.Configurations.Add(new User.Configuration());
            modelBuilder.Configurations.Add(new Request.Configuration());
            modelBuilder.Configurations.Add(new File.Configuration());
            modelBuilder.Configurations.Add(new Message.Configuration());
            modelBuilder.Configurations.Add(new ProjectAction.Configuration());
            modelBuilder.Configurations.Add(new AccessType.Configuration());
            modelBuilder.Configurations.Add(new UserLoginLog.Configuration());
            modelBuilder.Configurations.Add(new ProductName.Configuration());
            modelBuilder.Configurations.Add(new ProductType.Configuration());
            modelBuilder.Configurations.Add(new PackageType.Configuration());
            modelBuilder.Configurations.Add(new FactoryName.Configuration());
            modelBuilder.Configurations.Add(new Tonnage.Configuration());
            modelBuilder.Configurations.Add(new FactorCement.Configuration());
        }

        public System.Data.Entity.DbSet<Models.City> Cities { get; set; }

    }
}
