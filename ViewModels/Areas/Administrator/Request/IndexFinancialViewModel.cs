using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.Request
{
   public class IndexFinancialViewModel: System.Object
    {
       public IndexFinancialViewModel()
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

        #region RecordNumber
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

        #region PerformNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.PerformNumber)]
        #endregion
        public string PerformNumber { get; set; }

        #region PerformDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.PerformDate)]
        #endregion
        public string PerformDate { get; set; }

        #region CurrencyCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CurrencyCode)]
        #endregion
        public string CurrencyCode { get; set; }

        #region CurrencyValue
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.CurrencyValue)]
        #endregion
        public decimal CurrencyValue { get; set; }

        #region TotalValue
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.TotalValue)]
        #endregion
        public decimal TotalValue { get; set; }

        #region AmountPaid
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.AmountPaid)]
        #endregion
        public long AmountPaid { get; set; }

        #region Bank_BankReciptNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.BankReciptNumber)]
        #endregion
        public string Bank_BankReciptNumber { get; set; }

        #region Bank_TraceNo
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.TraceNo)]
        #endregion
        public string Bank_TraceNo { get; set; }

        #region Bank_ShamsiDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Bank.ShamsiDate)]
        #endregion
        public string Bank_ShamsiDate { get; set; }

        #region Tarefeh
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.Tarefeh)]
        #endregion
        public int Tarefeh { get; set; }

        #region SystemTarefeh
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.SystemTarefeh)]
        #endregion
        public int SystemTarefeh { get; set; }

        #region LisenceNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.LisenceNumber)]
        #endregion
        public string LisenceNumber { get; set; }

        #region LicenseDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.LicenseDate)]
        #endregion
        public string LicenseDate { get; set; }
    }
}
