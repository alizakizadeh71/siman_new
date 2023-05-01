using System.Linq;
using System.Data.Entity;
using System;

namespace Models
{
    public class FinancialManagement : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<FinancialManagement>
        {
            public Configuration()
            {
                HasRequired(current => current.User)
                     .WithMany(user => user.FinancialManagements)
                     .HasForeignKey(current => current.UserId)
                     .WillCascadeOnDelete(false)
                     ;

                HasRequired(current => current.ProductName)
                    .WithMany(productName => productName.FinancialManagements)
                    .HasForeignKey(current => current.ProductNameId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.ProductType)
                    .WithMany(productType => productType.FinancialManagements)
                    .HasForeignKey(current => current.ProductTypeId)
                    .WillCascadeOnDelete(false)
                    ;


                HasRequired(current => current.PackageType)
                    .WithMany(packageType => packageType.FinancialManagements)
                    .HasForeignKey(current => current.PackageTypeId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.FactoryName)
                    .WithMany(factoryName => factoryName.FinancialManagements)
                    .HasForeignKey(current => current.FactoryNameId)
                    .WillCascadeOnDelete(false)
                    ;
    
            }
        }

        #endregion

        public FinancialManagement()
        {
        }

        #region User
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.User)]
        #endregion
        public virtual User User { get; set; }

        #region UserId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.User)]
        #endregion
        public System.Guid UserId { get; set; }

        #region ProductName
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.ProductName)]
        #endregion
        public Guid ProductNameId { get; set; }
        public virtual ProductName ProductName { get; set; }

        #region ProductType
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.ProductType)]
        #endregion
        public Guid ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }


        #region PackageType
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.PackageType)]
        #endregion
        public Guid PackageTypeId { get; set; }
        public virtual PackageType PackageType { get; set; }

        #region FactoryName
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.FactoryName)]
        #endregion
        public Guid FactoryNameId { get; set; }
        public virtual FactoryName FactoryName { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.AmountPaid)]
        #endregion
        public long AmountPaid { get; set; }
    }
}
