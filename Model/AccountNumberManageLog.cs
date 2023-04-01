using System;
namespace Models
{
    public class AccountNumberManageLog : BaseExtendedEntity
    {
        #region Configuration
        internal class Configuration :
            System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<AccountNumberManageLog>
        {
            public Configuration()
            {
                Property(current => current.IPAddress).HasMaxLength(30);
            }
        }
        #endregion /Configuration

        public AccountNumberManageLog()
        {
        }

        public Guid OldId { get; set; }

        public Guid SubSystemId { get; set; }

        public Guid ProvinceId { get; set; }

        public Guid AccountNumberId { get; set; }

        public Guid UserId { get; set; }

        public string IPAddress { get; set; }
    }
}
