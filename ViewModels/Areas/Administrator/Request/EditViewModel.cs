using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.Request
{
   public class EditViewModel: System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

        #region SubSystemId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.SubSystem)]
        #endregion
        public string SubSystem { get; set; }

        #region SubSystemId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.ServiceTariff)]
        #endregion
        public string ServiceTariff { get; set; }

        #region CompanyName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CompanyName)]
        #endregion
        public string CompanyName { get; set; }

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

        #region RecordNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RecordNumber)]
        #endregion
        public string RecordNumber { get; set; }

        #region RecordDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RecordDate)]
        #endregion
        public string RecordDate { get; set; }

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

        #region Base Currency Value
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.BaseCurrencyValue)]
        #endregion
        public string BaseCurrencyValue { get; set; }

        #region DepositNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.DepositNumber)]
        #endregion
        public string DepositNumber { get; set; }

        #region Description
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Description)]
        #endregion
        public string Description { get; set; }

        #region Currency Value
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CurrencyValue)]
        #endregion
        public decimal CurrencyValue { get; set; }

        #region Currency Ration
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CurrencyRation)]
        #endregion
        public decimal? CurrencyRation { get; set; }

        #region AmountPaid
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.AmountPaid)]
        #endregion
        public long AmountPaid { get; set; }

        #region RequestState
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RequestState)]
        #endregion
        public string RequestState { get; set; }

        #region RequestStateCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.RequestState)]
        #endregion
        public int RequestStateCode { get; set; }

        #region Bank_TraceNo
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.TraceNo)]
        #endregion
        public long? Bank_TraceNo { get; set; }

        #region Bank_ShamsiDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.ShamsiDate)]
        #endregion
        public string Bank_ShamsiDate { get; set; }

        #region SystemMessage
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.SystemMessage)]
        #endregion
        public string SystemMessage { get; set; }

        #region Bank_ShamsiDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.ShamsiDate)]
        #endregion
        public System.DateTime? AmountPaidDate { get; set; }
    }
}
