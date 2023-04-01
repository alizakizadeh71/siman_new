namespace Models
{
    public class AccountNumber : BaseExtendedEntity
    {
        #region Configuration
        internal class Configuration :
            System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<AccountNumber>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(200);
                Property(current => current.Account).HasMaxLength(30);
                Property(current => current.TranKey).HasMaxLength(30);
                Property(current => current.MerchantId).HasMaxLength(30);
                Property(current => current.Terminal).HasMaxLength(30);

                HasRequired(current => current.Province)
                    .WithMany(province => province.AccountNumbers)
                    .HasForeignKey(current => current.ProvinceId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion /Configuration

        public AccountNumber()
        {
        }

        public System.Guid ProvinceId { get; set; }

        public virtual Province Province { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public string TranKey { get; set; }

        public string MerchantId { get; set; }

        public string Terminal { get; set; }

        public virtual System.Collections.Generic.IList<AccountNumberManage> AccountNumberManages { get; set; }
    }
}
