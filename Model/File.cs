namespace Models
{
    public class File : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<File>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(100);
                Property(current => current.FileAddress).HasMaxLength(500);

                HasRequired(current => current.Request)
                    .WithMany(request => request.Files)
                    .HasForeignKey(current => current.RequestId)
                    .WillCascadeOnDelete(true)
                    ;
            }
        }
        #endregion

        public File()
        { }

        public virtual Request Request { get; set; }

        public System.Guid RequestId { get; set; }

        public string Name { get; set; }

        public string FileAddress { get; set; }
    }
}
