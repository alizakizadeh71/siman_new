using System.Linq;
using System.Data.Entity;

namespace Models
{
    public class DetailOfFactor : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<DetailOfFactor>
        {
            public Configuration()
            {
                Property(current => current.CommodityDescription).HasMaxLength(100);
                
                HasRequired(current => current.ServiceTariff)
                    .WithMany(servicetariff => servicetariff.DetailOfFactors)
                    .HasForeignKey(current => current.ServiceTariffId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.HeadOfFactor)
                    .WithMany(servicetariff => servicetariff.DetailOfFactors)
                    .HasForeignKey(current => current.HeadOfFactorId)
                    .WillCascadeOnDelete(false)
                    ;

				//HasRequired(current => current.CurrencyUnit)
				//	.WithMany(servicetariff => servicetariff.DetailOfFactors)
				//	.WillCascadeOnDelete(false)
				//	;
			}
        }

        #endregion

        public DetailOfFactor()
        {
        }

        /// <summary>
        /// فاکتور
        /// </summary>
        #region OfficeService
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.HeadOfFactor)]
        #endregion
        public virtual HeadOfFactor HeadOfFactor { get; set; }

        #region HeadOfFactorId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.HeadOfFactor)]
        #endregion
        public System.Guid HeadOfFactorId { get; set; }

		//public virtual CurrencyUnit CurrencyUnit { get; set; }

		/// <summary>
		/// تعرفه خدمات
		/// </summary>
		#region OfficeService
		[System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.ServiceTariff)]
        #endregion
        public virtual ServiceTariff ServiceTariff { get; set; }

        #region OfficeServiceId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.ServiceTariff)]
        #endregion
        public System.Guid ServiceTariffId { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.CommodityDescription)]
        #endregion
        public string CommodityDescription { get; set; }
        
        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.CommodityCount)]
        #endregion
        public int CommodityCount { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.Description)]
        #endregion
        public string Description { get; set; }
        public System.Guid? CurrencyUnitId { get; set; }
        public decimal? CurrencyRatio { get; set; }


    }
}
