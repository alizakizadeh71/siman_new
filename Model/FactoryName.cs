using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// نام کارخانه
    /// </summary>
    public class FactoryName : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<FactoryName>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(30);
                Property(current => current.Code).HasMaxLength(2);

                HasRequired(current => current.ProductName)
                    .WithMany(office => office.FactoryNames)
                    .HasForeignKey(current => current.ProductNameId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion

        public FactoryName()
        { }
        
        public string Name { get; set; }

        public string Code { get; set; }

        public virtual ProductName ProductName { get; set; }

        public System.Guid ProductNameId { get; set; }

        public virtual System.Collections.Generic.IList<PackageType> PackageTypes { get; set; }
    }
}
