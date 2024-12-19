using System;

namespace ViewModels.Areas.Administrator.Request
{
    public class SearchViewModel : System.Object
    {
        public SearchViewModel()
        { }

        #region PerformNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.PerformNumber)]
        #endregion
        public string PerformNumber { get; set; }

        #region CompanyName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CompanyName)]
        #endregion
        public string CompanyName { get; set; }

        #region DepositNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.DepositNumber)]
        #endregion
        public string DepositNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.Request),
        Name = Resources.Model.Strings.RequestKeys.Bank.Terminal)]
        public string Bank_Terminal { get; set; }


        #region CompanyNationalCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CompanyNationalCode)]
        #endregion
        public string CompanyNationalCode { get; set; }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Province)]
        #endregion
        public System.Guid? Province { get; set; }

        #region ServiceTariffs
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.ServiceTariff)]
        #endregion
        public System.Guid? ServiceTariffs { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.City)]
        #endregion
        public System.Guid? City { get; set; }

        #region SubSystem
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.SubSystem)]
        #endregion
        public Guid SubSystem { get; set; }

        #region RecordCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RecordNumber)]
        #endregion
        public string RecordNumber { get; set; }

        #region RecordCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.ImportRecordNumber2)]
        #endregion
        public string ImportRecordNumber2 { get; set; }

        #region InvoiceNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.InvoiceNumber)]
        #endregion
        public int? InvoiceNumber { get; set; }

        #region RequestState
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RequestState)]
        #endregion
        public int? RequestState { get; set; }

        #region StartDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.StartDate)]
        #endregion
        public DateTime? StartDate { get; set; }

        #region EndDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.EndDate)]
        #endregion
        public DateTime? EndDate { get; set; }

        #region StartDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.PayStartDate)]
        #endregion
        public DateTime? PayStartDate { get; set; }

        #region EndDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.PayEndDate)]
        #endregion
        public DateTime? PayEndDate { get; set; }

        #region FromAmount
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.FromAmount)]
        #endregion
        public int? FromAmount { get; set; }

        #region ToAmount
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.ToAmount)]
        #endregion
        public int? ToAmount { get; set; }

        #region CommodityType
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CommodityType)]
        #endregion
        public string CommodityType { get; set; }

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
    }
}
