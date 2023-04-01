using System;
namespace Models
{
    public class AccountNumberManage : BaseExtendedEntity
    {
        #region Configuration
        internal class Configuration :
            System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<AccountNumberManage>
        {
            public Configuration()
            {
                HasRequired(current => current.SubSystem)
                    .WithMany(role => role.AccountNumberManages)
                    .HasForeignKey(current => current.SubSystemId)
                    .WillCascadeOnDelete(true)
                    ;

                HasRequired(current => current.Province)
                    .WithMany(role => role.AccountNumberManages)
                    .HasForeignKey(current => current.ProvinceId)
                    .WillCascadeOnDelete(true)
                    ;

                HasRequired(current => current.AccountNumber)
                    .WithMany(role => role.AccountNumberManages)
                    .HasForeignKey(current => current.AccountNumberId)
                    .WillCascadeOnDelete(true)
                    ;
            }
        }
        #endregion /Configuration

        public AccountNumberManage()
        {
        }

        public Guid SubSystemId { get; set; }

        public virtual SubSystem SubSystem { get; set; }

        public Guid ProvinceId { get; set; }

        public virtual Province Province { get; set; }

        public Guid AccountNumberId { get; set; }

        public virtual AccountNumber AccountNumber { get; set; }
    }
}
