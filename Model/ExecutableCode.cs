namespace Models
{
    /// <summary>
    /// دستگاه اجرایی
    /// </summary>
    public class ExecutableCode : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ExecutableCode>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(50);
                Property(current => current.Code).HasMaxLength(4);
            }
        }
        #endregion

        public ExecutableCode()
        { }
        
        public string Name { get; set; }

        public string Code { get; set; }

        public virtual System.Collections.Generic.IList<BankAccount> BankAccounts { get; set; }
    }
}
