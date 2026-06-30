using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("CarrierInventory")]
    public class CarrierInventory : BaseExtendedEntity
    {
        [Required]
        public Guid ProductNameId { get; set; }

        [Required]
        public Guid ProductTypeId { get; set; }

        [Required]
        public Guid PackageTypeId { get; set; }

        [Required]
        public Guid FactoryNameId { get; set; }

        [Required]
        public Guid CarrierId { get; set; }

        [Required]
        public double InventoryTonnage { get; set; }

        [Required]
        public bool IsDefaultCarrier { get; set; }

        [ForeignKey(nameof(ProductNameId))]
        public virtual ProductName ProductName { get; set; }

        [ForeignKey(nameof(ProductTypeId))]
        public virtual ProductType ProductType { get; set; }

        [ForeignKey(nameof(PackageTypeId))]
        public virtual PackageType PackageType { get; set; }

        [ForeignKey(nameof(FactoryNameId))]
        public virtual FactoryName FactoryName { get; set; }

        [ForeignKey(nameof(CarrierId))]
        public virtual User Carrier { get; set; }

        public class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<CarrierInventory>
        {
            public Configuration()
            {
                HasRequired(x => x.ProductName)
                    .WithMany()
                    .HasForeignKey(x => x.ProductNameId)
                    .WillCascadeOnDelete(false);

                HasRequired(x => x.ProductType)
                    .WithMany()
                    .HasForeignKey(x => x.ProductTypeId)
                    .WillCascadeOnDelete(false);

                HasRequired(x => x.PackageType)
                    .WithMany()
                    .HasForeignKey(x => x.PackageTypeId)
                    .WillCascadeOnDelete(false);

                HasRequired(x => x.FactoryName)
                    .WithMany()
                    .HasForeignKey(x => x.FactoryNameId)
                    .WillCascadeOnDelete(false);

                HasRequired(x => x.Carrier)
                    .WithMany()
                    .HasForeignKey(x => x.CarrierId)
                    .WillCascadeOnDelete(false);
            }
        }
    }
}