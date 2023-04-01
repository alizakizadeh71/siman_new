namespace Models
{
    public class UserLoginLog : BaseEntity
    {
        #region Configuration
        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UserLoginLog>
        {
            public Configuration()
            {
                Property(current => current.SessionId).HasMaxLength(200);
                Property(current => current.IP).HasMaxLength(30);

                HasRequired(current => current.User)
                    .WithMany(user => user.UserLoginLogs)
                    .HasForeignKey(current => current.UserId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion

        public UserLoginLog()
        {
            LogoutDateTime = System.DateTime.Now;
            LoginDateTime = System.DateTime.Now;
        }
        public System.Guid UserId { get; protected internal set; }
        public virtual User User { get; protected internal set; }

        public string SessionId { get; set; }

        public string IP { get; set; }

        public System.DateTime? LoginDateTime { get; set; }

        public System.DateTime? LogoutDateTime { get; set; }

    }
}
