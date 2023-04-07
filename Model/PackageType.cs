using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// نوع بسته بندی
    /// </summary>
    public class PackageType : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PackageType>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(30);
                Property(current => current.Code).HasMaxLength(2);

                HasRequired(current => current.ProductType)
                    .WithMany(office => office.PackageTypes)
                    .HasForeignKey(current => current.ProductTypeId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion

        public PackageType()
        { }
        
        public string Name { get; set; }

        public string Code { get; set; }

        public virtual ProductType ProductType { get; set; }

        public System.Guid ProductTypeId { get; set; }
        public virtual System.Collections.Generic.IList<Tonnage> Tonnages { get; set; }
        public virtual System.Collections.Generic.IList<FactorCement> FactorCements { get; set; }
    }
}
