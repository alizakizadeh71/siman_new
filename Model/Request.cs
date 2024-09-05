using System.Linq;
using System.Data.Entity;

namespace Models
{
    public class Request : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Request>
        {
            public Configuration()
            {
                Property(current => current.InvoiceNumber)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

                Property(current => current.LicenseNumber).HasMaxLength(20);
                Property(current => current.OrganDigitCode).HasMaxLength(13);
                Property(current => current.BankDigitCode).HasMaxLength(17);
                Property(current => current.DepositNumber).HasMaxLength(30);
                Property(current => current.CommodityType).HasMaxLength(1000);
                Property(current => current.CommodityUnit).HasMaxLength(100);
                Property(current => current.SecNumber).HasMaxLength(20);
                Property(current => current.SecDate).HasMaxLength(30);
                Property(current => current.PerformNumber).HasMaxLength(40);
                Property(current => current.PerformDate).HasMaxLength(30);
                Property(current => current.CompanyName).HasMaxLength(100);
                Property(current => current.CompanyNationalCode).HasMaxLength(11);
                Property(current => current.RecordNumber).HasMaxLength(20);
                Property(current => current.ImportRecordNumber).HasMaxLength(20);
                Property(current => current.RecordDate).HasMaxLength(10);
                Property(current => current.CellPhoneNumber).HasMaxLength(11);

                Property(current => current.Bank_RefrenceNumber).HasMaxLength(50);
                Property(current => current.Bank_BankReciptNumber).HasMaxLength(50);
                Property(current => current.Bank_ResponseCode).HasMaxLength(100);
                Property(current => current.Bank_ShamsiDate).HasMaxLength(15);
                Property(current => current.Bank_AppStatusDescription).HasMaxLength(300);
                Property(current => current.Bank_CustomerCardNumber).HasMaxLength(30);
                Property(current => current.Bank_CardHolderAccNumber).HasMaxLength(30);
                Property(current => current.Bank_CardHolderName).HasMaxLength(50);
                Property(current => current.Bank_RequestKey).HasMaxLength(50);
                Property(current => current.Bank_MerchantId).HasMaxLength(20);
                Property(current => current.Bank_Terminal).HasMaxLength(20);
                Property(current => current.VirtualCode).HasMaxLength(50);

                HasOptional(current => current.ServiceTariff)
                    .WithMany(servicetariff => servicetariff.Requests)
                    .HasForeignKey(current => current.ServiceTariffId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.User)
                    .WithMany(user => user.Requests)
                    .HasForeignKey(current => current.UserId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.Province)
                    .WithMany(user => user.Requests)
                    .HasForeignKey(current => current.ProvinceId)
                    .WillCascadeOnDelete(false)
                    ;

                HasOptional(current => current.City)
                    .WithMany(user => user.Requests)
                    .HasForeignKey(current => current.CityId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.SubSystem)
                    .WithMany(user => user.Requests)
                    .HasForeignKey(current => current.SubSystemId)
                    .WillCascadeOnDelete(false)
                    ;
			}
        }

        #endregion

        public Request()
        {
        }

        public virtual HeadOfFactor HeadOfFactor { get; set; }

        /// <summary>
        /// تعرفه خدمات
        /// </summary>
        #region OfficeService
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.ServiceTariff)]
        #endregion
        public virtual ServiceTariff ServiceTariff { get; set; }

        /// <summary>
        /// تعرفه خدمات
        /// </summary>
        #region OfficeServiceId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.OfficeService)]
        #endregion
        public System.Guid? ServiceTariffId { get; set; }

        /// <summary>
        /// کد مجازی سیستم 
        /// </summary>
        #region Organ Digit Code
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.VirtualCode)]
        #endregion
        public string VirtualCode { get; set; }

        /// <summary>
        /// کد 13 رقمی 
        /// </summary>
        #region Organ Digit Code
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.OrganDigitCode)]
        #endregion
        public string OrganDigitCode { get; set; }

        /// <summary>
        /// کد 17 رقمی 
        /// </summary>
        #region Bank Digit Code
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.BankDigitCode)]
        #endregion
        public string BankDigitCode { get; set; }

        /// <summary>
        /// شناسه واریز 
        /// </summary>
        #region Deposit ID
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.DepositNumber)]
        #endregion
        public string DepositNumber { get; set; }

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

        #region Subsystem
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.SubSystem)]
        #endregion
        public virtual SubSystem SubSystem { get; set; }


		#region SubSystemId
		[System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.SubSystem)]
        #endregion
        public System.Guid SubSystemId { get; set; }

		#region Province
		[System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Province)]
        #endregion
        public virtual Province Province { get; set; }

        #region ProvinceId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Province)]
        #endregion
        public System.Guid ProvinceId { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.City)]
        #endregion
        public virtual City City { get; set; }

        #region CityId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.City)]
        #endregion
        public System.Guid? CityId { get; set; }

        #region CommodityType
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CommodityType)]
        #endregion
        public string CommodityType { get; set; }

        #region TotalValue
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.TotalValue)]
        #endregion
        public decimal TotalValue { get; set; }

        #region CommodityUnit
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CommodityUnit)]
        #endregion
        public string CommodityUnit { get; set; }

        #region SecNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.SecNumber)]
        #endregion
        public string SecNumber { get; set; }

        #region SecDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.SecDate)]
        #endregion
        public string SecDate { get; set; }

        #region PerformNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.PerformNumber)]
        #endregion
        public string PerformNumber { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.PerformDate)]
        #endregion
        public string PerformDate { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CompanyName)]
        #endregion
        public string CompanyName { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CompanyNationalCode)]
        #endregion
        public string CompanyNationalCode { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RecordNumber)]
        #endregion
        public string RecordNumber { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RecordDate)]
        #endregion
        public string RecordDate { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.ImportRecordNumber)]
        #endregion
        public string ImportRecordNumber { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.InvoiceNumber)]
        #endregion
        public int InvoiceNumber { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.InvoiceDate)]
        #endregion
        public System.DateTime InvoiceDate { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CellPhoneNumber)]
        #endregion
        public string CellPhoneNumber { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CurrencyCode)]
        #endregion
        public int CurrencyCode { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CurrencyValue)]
        #endregion
        public decimal CurrencyValue { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CurrencyRation)]
        #endregion
        public decimal? CurrencyRation { get; set; }


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
            Name = Resources.Model.Strings.RequestKeys.BaseCurrencyValue)]
        #endregion
        public decimal? BaseCurrencyValue { get; set; }

        public string URLAddress { get; set; }
        public string UserIPAddress { get; set; }
        public string Browser { get; set; }
		public decimal? Ratio { get; set; }


		#region
		[System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Description)]
        #endregion
        public string Description { get; set; }

        public int? Tariffs { get; set; }

        public string LicenseNumber { get; set; }
        #region RemittanceNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RemittanceNumber)]
        #endregion
        public string RemittanceNumber { get; set; }
        public System.DateTime? LicenseDate { get; set; }

        #region Bank Data Filed

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.RefrenceNumber)]
        public string Bank_RefrenceNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.BankReciptNumber)]
        public string Bank_BankReciptNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.NewlyCommitted)]
        public bool? Bank_NewlyCommitted { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.FailCode)]
        public int? Bank_FailCode { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.ResponseCode)]
        public string Bank_ResponseCode { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.TraceNo)]
        public long? Bank_TraceNo { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.ShamsiDate)]
        public string Bank_ShamsiDate { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.RealTransactionDateTime)]
        public System.DateTime? Bank_RealTransactionDateTime { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.AppStatus)]
        public int? Bank_AppStatus { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.AppStatusDescription)]
        public string Bank_AppStatusDescription { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.AppStatusCode)]
        public int? Bank_AppStatusCode { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.CustomerCardNumber)]
        public string Bank_CustomerCardNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.CardHolderAccNumber)]
        public string Bank_CardHolderAccNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.CardHolderName)]
        public string Bank_CardHolderName { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.RequestKey)]
        public string Bank_RequestKey { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.MerchantId)]
        public string Bank_MerchantId { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.Terminal)]
        public string Bank_Terminal { get; set; }

        public System.DateTime? AmountPaidDate { get; set; }

        public bool TestBit { get; set; }

        #endregion

        public virtual System.Collections.Generic.IList<File> Files { get; set; }
        public virtual System.Collections.Generic.IList<Message> Messages { get; set; }

    }
}
