namespace Models.Migrations
{
    internal sealed class Configuration : System.Data.Entity.Migrations.DbMigrationsConfiguration<Models.DatabaseContext>
    {
        public Configuration()
        {
            ContextKey = "Models.DatabaseContext";

            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DatabaseContext databaseContext)
        {
            try
            {
                //DatabaseContextInitializer.Seed(databaseContext);
            }
            catch (System.Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
            }
        }
    }
}
