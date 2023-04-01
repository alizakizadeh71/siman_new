
namespace Models
{
    public class ErrorLog : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ErrorLog>
        {
            public Configuration()
            {
                Property(current => current.UserName).HasMaxLength(100);
                Property(current => current.Description2).HasMaxLength(200);

            }
        }
        #endregion

       public ErrorLog()
        {
        }

        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
    }
}
