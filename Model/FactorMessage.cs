namespace Models
{
    public class FactorMessage : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<FactorMessage>
        {
            public Configuration()
            {
                Property(current => current.MessageText).HasMaxLength(1000);

                HasRequired(current => current.HeadOfFactor)
                    .WithMany(headoffactor => headoffactor.FactorMessages)
                    .HasForeignKey(current => current.HeadOfFactorId)
                    .WillCascadeOnDelete(true)
                    ;

                HasRequired(current => current.User)
                    .WithMany(headoffactor => headoffactor.FactorMessages)
                    .HasForeignKey(current => current.UserId)
                    .WillCascadeOnDelete(true)
                    ;
            }
        }
        #endregion

        public FactorMessage()
        { }

        public virtual HeadOfFactor HeadOfFactor { get; set; }

        public System.Guid HeadOfFactorId { get; set; }

        public virtual User User { get; set; }

        public System.Guid UserId { get; set; }

        public string MessageText { get; set; }
    }
}
