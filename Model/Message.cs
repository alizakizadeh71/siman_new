namespace Models
{
    public class Message : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Message>
        {
            public Configuration()
            {
                Property(current => current.MessageText).HasMaxLength(1000);

                HasRequired(current => current.Request)
                    .WithMany(request => request.Messages)
                    .HasForeignKey(current => current.RequestId)
                    .WillCascadeOnDelete(true)
                    ;

                HasRequired(current => current.User)
                    .WithMany(request => request.Messages)
                    .HasForeignKey(current => current.UserId)
                    .WillCascadeOnDelete(true)
                    ;
            }
        }
        #endregion

        public Message()
        { }

        public virtual Request Request { get; set; }

        public System.Guid RequestId { get; set; }

        public virtual User User { get; set; }

        public System.Guid UserId { get; set; }

        public string MessageText { get; set; }

        public int LastState { get; set; }

        public int NewState { get; set; }

		public string UserIPAddress { get; set; }
		public string Browser { get; set; }
	}
}
