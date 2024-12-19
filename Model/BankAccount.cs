namespace Models
{
    /// <summary>
    /// عنوان حساب - شامل : تمامی اطلاعات پایه برای کد 17 رقمی
    /// </summary>
    public class BankAccount : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<BankAccount>
        {
            public Configuration()
            {
                Property(current => current.AccountTitel).HasMaxLength(200);
                Property(current => current.AccountNumber).HasMaxLength(40);

                HasRequired(current => current.Bank)
                    .WithMany(x => x.BankAccounts)
                    .HasForeignKey(current => current.BankId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.ExecutableCode)
                    .WithMany(x => x.BankAccounts)
                    .HasForeignKey(current => current.ExecutableCodeId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.IncomeRow)
                    .WithMany(x => x.BankAccounts)
                    .HasForeignKey(current => current.IncomeRowId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.Certain)
                    .WithMany(x => x.BankAccounts)
                    .HasForeignKey(current => current.CertainId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }

        #endregion

        public BankAccount()
        {
        }

        /// <summary>
        /// بانک
        /// </summary>
        #region Bank
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.BankAccount),
            Name = Resources.Model.Strings.BankAccountKeys.Bank)]
        #endregion
        public virtual Bank Bank { get; set; }

        /// <summary>
        /// بانک
        /// </summary>
        #region Bank
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.BankAccount),
            Name = Resources.Model.Strings.BankAccountKeys.Bank)]
        #endregion
        public System.Guid BankId { get; set; }

        /// <summary>
        /// دستگاه اجرایی
        /// </summary>
        #region ExecutableCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.BankAccount),
            Name = Resources.Model.Strings.BankAccountKeys.ExecutableCode)]
        #endregion
        public virtual ExecutableCode ExecutableCode { get; set; }

        /// <summary>
        /// دستگاه اجرایی
        /// </summary>
        #region ExecutableCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.BankAccount),
            Name = Resources.Model.Strings.BankAccountKeys.ExecutableCode)]
        #endregion
        public System.Guid ExecutableCodeId { get; set; }

        /// <summary>
        /// ردیف درآمدی
        /// </summary>
        #region IncomeRow
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.BankAccount),
            Name = Resources.Model.Strings.BankAccountKeys.IncomeRow)]
        #endregion
        public virtual IncomeRow IncomeRow { get; set; }

        /// <summary>
        /// ردیف درآمدی
        /// </summary>
        #region IncomeRow
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.BankAccount),
            Name = Resources.Model.Strings.BankAccountKeys.IncomeRow)]
        #endregion
        public System.Guid IncomeRowId { get; set; }

        /// <summary>
        /// کد معین
        /// </summary>
        #region Certain
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.BankAccount),
            Name = Resources.Model.Strings.BankAccountKeys.Certain)]
        #endregion
        public virtual Certain Certain { get; set; }

        /// <summary>
        /// کد معین
        /// </summary>
        #region Certain
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.BankAccount),
            Name = Resources.Model.Strings.BankAccountKeys.Certain)]
        #endregion
        public System.Guid CertainId { get; set; }

        /// <summary>
        /// عنوان حساب 
        /// </summary>
        #region Account Titel
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.BankAccount),
            Name = Resources.Model.Strings.BankAccountKeys.AccountTitel)]
        #endregion
        public string AccountTitel { get; set; }

        /// <summary>
        /// شماره حساب تمرکزی 
        /// </summary>
        #region Account Number
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.BankAccount),
            Name = Resources.Model.Strings.BankAccountKeys.AccountNumber)]
        #endregion
        public string AccountNumber { get; set; }

        public virtual System.Collections.Generic.IList<ServiceTariff> ServiceTariffs { get; set; }

    }
}
