namespace Models
{
    /// <summary>
    /// بانک
    /// </summary>
    public class Bank : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Bank>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(50);
                Property(current => current.Code).HasMaxLength(1);
            }
        }
        #endregion

        public Bank()
        { }
        
        public string Name { get; set; }

        public string Code { get; set; }

        public virtual System.Collections.Generic.IList<BankAccount> BankAccounts { get; set; }
    }
}
