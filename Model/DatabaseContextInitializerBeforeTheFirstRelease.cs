namespace Models
{
    class DatabaseContextInitializerBeforeTheFirstRelease : System.Data.Entity.CreateDatabaseIfNotExists<DatabaseContext>
    {
        public DatabaseContextInitializerBeforeTheFirstRelease()
        { }

        protected override void Seed(DatabaseContext databaseContext)
        {
            //base.Seed(databaseContext);

            try 
            {
               // DatabaseContextInitializer.Seed(databaseContext);
            }

            catch(System.Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
            }
        }
    }
}
