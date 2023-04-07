using System.Linq;
using System.Data.Entity;
using System;

namespace Models
{
    public class FactorCement : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<FactorCement>
        {
            public Configuration()
            {
                Property(current => current.InvoiceNumber)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

                Property(current => current.DepositNumber).HasMaxLength(30);
                Property(current => current.BuyerMobile).HasMaxLength(11);

                HasRequired(current => current.User)
                     .WithMany(user => user.FactorCements)
                     .HasForeignKey(current => current.UserId)
                     .WillCascadeOnDelete(false)
                     ;

                //HasOptional(current => current.ProductName)
                //    .WithMany(productName => productName.FactorCements)
                //    .HasForeignKey(current => current.ProductNameId)
                //    .WillCascadeOnDelete(false)
                //    ;

                //HasOptional(current => current.ProductType)
                //    .WithMany(productType => productType.FactorCements)
                //    .HasForeignKey(current => current.ProductTypeId)
                //    .WillCascadeOnDelete(false)
                //    ;


                //HasOptional(current => current.PackageType)
                //    .WithMany(packageType => packageType.FactorCements)
                //    .HasForeignKey(current => current.PackageType)
                //    .WillCascadeOnDelete(false)
                //    ;

                //HasOptional(current => current.FactoryName)
                //    .WithMany(factoryName => factoryName.FactorCements)
                //    .HasForeignKey(current => current.FactoryNameId)
                //    .WillCascadeOnDelete(false)
                //    ;

                //HasOptional(current => current.Tonnage)
                //    .WithMany(tonnage => tonnage.FactorCements)
                //    .HasForeignKey(current => current.TonnageId)
                //    .WillCascadeOnDelete(false)
                //    ;


                //HasRequired(current => current.Province)
                //    .WithMany(user => user.FactorCements)
                //    .HasForeignKey(current => current.ProvinceId)
                //    .WillCascadeOnDelete(false)
                //    ;

                HasRequired(current => current.City)
                    .WithMany(user => user.FactorCements)
                    .HasForeignKey(current => current.CityId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }

        #endregion

        public FactorCement()
        {
        }
        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.InvoiceNumber)]
        #endregion
        public int InvoiceNumber { get; set; }


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
        //public virtual ProductName ProductName { get; set; }

        #region ProductType
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.ProductType)]
        #endregion
        public Guid ProductTypeId { get; set; }
        //public virtual ProductType ProductType { get; set; }


        #region PackageType
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.PackageType)]
        #endregion
        public Guid PackageTypeId { get; set; }
        //public virtual PackageType PackageType { get; set; }

        #region FactoryName
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.FactoryName)]
        #endregion
        public Guid FactoryNameId { get; set; }
        //public virtual FactoryName FactoryName { get; set; }

        #region Tonnage
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.Tonnage)]
        #endregion
        public Guid TonnageId { get; set; }
        //public virtual Tonnage Tonnage { get; set; }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.Province)]
        #endregion
        public Guid ProvinceId { get; set; }
        //public virtual Province Province { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.City)]
        #endregion
        public Guid CityId { get; set; }
        public virtual City City { get; set; }

        //#region Village
        //[System.ComponentModel.DataAnnotations.Display
        //     (ResourceType = typeof(Resources.Model.Cement),
        //     Name = Resources.Model.Strings.CementKeys.Village)]
        //#endregion
        //public Guid? Village { get; set; }

        #region BuyerMobile
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.BuyerMobile)]
        #endregion
        public string BuyerMobile { get; set; }

        #region Address
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.Address)]
        #endregion
        public string Address { get; set; }

        /// <summary>
        /// شناسه واریز 
        /// </summary>
        #region Deposit ID
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.DepositNumber)]
        #endregion
        public string DepositNumber { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.AmountPaid)]
        #endregion
        public long AmountPaid { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RequestState)]
        #endregion
        public int RequestState { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Description)]
        #endregion
        public string Description { get; set; }

        public string URLAddress { get; set; }
        public string UserIPAddress { get; set; }
        public string Browser { get; set; }

        public virtual System.Collections.Generic.IList<File> Files { get; set; }
        public virtual System.Collections.Generic.IList<Message> Messages { get; set; }

    }
}
