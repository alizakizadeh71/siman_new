using System;

namespace ViewModels.Areas.Administrator.Request
{
    public class IndexViewModel : System.Object
    {
        public IndexViewModel()
        { }

        public System.Guid Id { get; set; }

        #region SubSystem
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.SubSystem)]
        #endregion
        public string SubSystem { get; set; }

        #region CompanyName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CompanyName)]
        #endregion
        public string CompanyName { get; set; }

        #region CommodityType
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CommodityType)]
        #endregion
        public string CommodityType { get; set; }

        #region CommodityType
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.ServiceTariff)]
        #endregion
        public string ServiceTariff { get; set; }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Province)]
        #endregion
        public string Province { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.City)]
        #endregion
        public string City { get; set; }

        #region CompanyNationalCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CompanyNationalCode)]
        #endregion
        public string CompanyNationalCode { get; set; }

        #region InvoiceNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.InvoiceNumber)]
        #endregion
        public int InvoiceNumber { get; set; }

        #region InvoiceDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.InvoiceDate)]
        #endregion
        public string InvoiceDate { get; set; }

        #region InvoiceDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.AmountPaidDate)]
        #endregion
        public string AmountPaidDate { get; set; }

        #region RequestCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RecordNumber)]
        #endregion
        public string RecordNumber { get; set; }

        #region RequestDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RecordDate)]
        #endregion
        public string RecordDate { get; set; }

        public string SecNumber { get; set; }

        #region RequestState
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RequestState)]
        #endregion
        public string RequestState { get; set; }

        #region AmountPaid
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.AmountPaid)]
        #endregion
        public long AmountPaid { get; set; }

        public int RequestState_Value { get; set; }

        #region DepositNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.DepositNumber)]
        #endregion
        public string DepositNumber { get; set; }
        public long Token { get; set; }


        #region Bank_TraceNo
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.TraceNo)]
        #endregion
        public long? Bank_TraceNo { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.BaseCurrencyValue)]
        #endregion
        public decimal? BaseCurrencyValue { get; set; }

        #region Description
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CurrencyUnit)]
        #endregion
        public Guid? CurrencyUnit { get; set; }
        public string CeratedBy { get; set; }
        public DateTime? InvoiceDateNew { get; set; }
        public DateTime? AmountPaidDateNew { get; set; }
        public string Bank_ShamsiDate { get; set; }
        public int? RequestStateNew { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public Guid SubSystemId { get; set; }
        public Guid? ServiceTariffId { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid? CityId { get; set; }
        public Guid CeratedById { get; set; }
    }
}
