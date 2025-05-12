using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// تناژ
    /// </summary>
    public class Tonnage : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Tonnage>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(30);
                Property(current => current.Code).HasMaxLength(2);

                HasRequired(current => current.PackageType)
                    .WithMany(office => office.Tonnages)
                    .HasForeignKey(current => current.PackageTypeId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion

        public Tonnage()
        { }
        
        public string Name { get; set; }

        public string Code { get; set; }

        public virtual PackageType PackageType { get; set; }

        public System.Guid PackageTypeId { get; set; }
        public virtual System.Collections.Generic.IList<FactorCement> FactorCements { get; set; }
    }
}
