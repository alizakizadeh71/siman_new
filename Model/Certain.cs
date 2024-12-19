namespace Models
{
    /// <summary>
    /// کد معین
    /// </summary>
    public class Certain : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Certain>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(50);
                Property(current => current.Code).HasMaxLength(2);
            }
        }
        #endregion

        public Certain()
        { }

        public string Name { get; set; }

        public string Code { get; set; }

        public virtual System.Collections.Generic.IList<BankAccount> BankAccounts { get; set; }
    }
}
