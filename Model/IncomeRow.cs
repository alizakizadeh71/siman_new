namespace Models
{
    /// <summary>
    /// ردیف درآمدی
    /// </summary>
    public class IncomeRow : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<IncomeRow>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(100);
                Property(current => current.Code).HasMaxLength(6);
            }
        }
        #endregion

        public IncomeRow()
        { }

        public string Name { get; set; }

        public string Code { get; set; }

        public virtual System.Collections.Generic.IList<BankAccount> BankAccounts { get; set; }
    }
}
