using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Areas.Administrator.Request
{
    public class IndexViewModel : System.Object
    {
        public IndexViewModel()
        { }

        public Guid Id { get; set; }

        #region شماره فاکتور و تاریخ
        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.InvoiceNumber)]
        public int? InvoiceNumber { get; set; }

        public DateTime? InsertDateTime { get; set; }
        public string StringInsertDateTime { get; set; }
        #endregion

        #region فیلدهای جستجو و دراپ‌داون‌ها (Guid)
        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.ProductName)]
        public Guid? ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.ProductType)]
        public Guid? ProductType { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.PackageType)]
        public Guid? PackageType { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.FactoryName)]
        public Guid? FactoryName { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.Province)]
        public Guid? Province { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.City)]
        public Guid? City { get; set; }
        #endregion

        #region فیلدهای نمایشی گرید (String)
        public string StringProductName { get; set; }
        public string StringProductType { get; set; }
        public string StringPackageType { get; set; }
        public string StringFactoryName { get; set; }
        public string StringProvince { get; set; }
        public string StringCity { get; set; }
        public string StringTonnage { get; set; }
        #endregion

        #region اطلاعات خریدار و سفارش
        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.BuyerMobile)]
        [MaxLength(11, ErrorMessage = "شماره همراه باید 11 رقم باشد")]
        public string BuyerMobile { get; set; }

        public string BuyerNationalCode { get; set; }
        public string BuyerName { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.Address)]
        public string Address { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.Description)]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.MahalTahvil)]
        public string MahalTahvil { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.Tonnage)]
        public double Tonnage { get; set; }
        #endregion

        #region وضعیت تایید
        // === این فیلد اضافه شد ===
        [Display(Name = "وضعیت درخواست")]
        public int RequestState { get; set; }
        // ==========================

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.FinalApprove)]
        public bool? FinalApprove { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.FinalApprove)]
        public string stringFinalApprove { get; set; } // برای فرم جستجو (0 یا 1) و نمایش گرید ("نهایی شده" / "نهایی نشده")
        #endregion

        #region اطلاعات مالی و بانکی
        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.AmountPaid)]
        public double AmountPaid { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Cement), Name = Resources.Model.Strings.CementKeys.DestinationAmountPaid)]
        public double? DestinationAmountPaid { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.RemittanceNumber)]
        public string RemittanceNumber { get; set; }

        public string ref_id { get; set; }
        public string card_pan { get; set; }
        #endregion

        #region فیلترهای بازه زمانی و مبلغ (برای فرم جستجو)
        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.FromAmount)]
        public int? FromAmount { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.ToAmount)]
        public int? ToAmount { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.StartDate)]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.EndDate)]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.PayStartDate)]
        public DateTime? PayStartDate { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.PayEndDate)]
        public DateTime? PayEndDate { get; set; }
        #endregion

        #region اطلاعات باربری (Driver Information)

        public string CarrierName { get; set; }
        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.DriverName)]
        public string DriverName { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.DriverLastName)]
        public string DriverLastName { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.DriverMobile)]
        [StringLength(11, ErrorMessage = "شماره موبایل باید 11 رقم باشد")]
        public string DriverMobile { get; set; }

        [Display(ResourceType = typeof(Resources.Model.Request), Name = Resources.Model.Strings.RequestKeys.DriverLicensePlate)]
        public string DriverLicensePlate { get; set; }
        #endregion
    }
}
