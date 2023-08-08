using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.Cement
{
    public class CementViewModel : System.Object
    {
        public CementViewModel()
        { }

        #region ProductName
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.ProductName)]
        #endregion
        public Guid ProductName { get; set; }
        public Guid ProductName1 { get; set; }
        public string ProductName2 { get; set; }
        #region IsActive
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.IsActive)]
        #endregion IsActive
        public bool IsActive { get; set; }

        #region News
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.News)]
        #endregion News
        public Guid News { get; set; }

        #region TitleNews
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.TitleNews)]
        #endregion TitleNews

        public string TitleNews { get; set; }

        #region TextNews
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.TextNews)]
        #endregion TextNews
        public string TextNews { get; set; }

        #region TextNews
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.startDateNews)]
        #endregion TextNews
        public DateTime startDateNews { get; set; }
        public string startDateNews1 { get; set; }

        #region TextNews
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.EndDateNews)]
        #endregion TextNews
        public DateTime EndDateNews { get; set; }
        public string EndDateNews1 { get; set; }
        #region ProductName
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.code)]
        #endregion
        public string code { get; set; }
        #region ProductType
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.ProductType)]
        #endregion
        public Guid ProductType { get; set; }
        public Guid ProductType1 { get; set; }

        #region PackageType
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.PackageType)]
        #endregion
        public Guid PackageType { get; set; }
        public Guid PackageType1 { get; set; }

        #region FactoryName
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.FactoryName)]
        #endregion
        public Guid FactoryName { get; set; }
        public Guid FactoryName1 { get; set; }

        #region Tonnage
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.Tonnage)]
        #endregion
        public Guid Tonnage { get; set; }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.Province)]
        #endregion
        public Guid Province { get; set; }
        public Guid Province1 { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.City)]
        #endregion
        public Guid City { get; set; }
        public Guid City1 { get; set; }

        #region Village
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.Village)]
        #endregion
        public Guid? Village { get; set; }

        #region BuyerMobile
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.BuyerMobile)]
        [System.ComponentModel.DataAnnotations.MaxLength(11, ErrorMessage = "شماره همراه باید 11 رقم باشد")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        #endregion
        public string BuyerMobile { get; set; }
        public string BuyerNationalCode { get; set; }
        public string BuyerName { get; set; }

        #region Address
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.Address)]
        #endregion
        public string Address { get; set; }
        public string Description { get; set; }

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

        #region InvoiceNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.InvoiceNumber)]
        #endregion
        public int? InvoiceNumber { get; set; }
        public string StringProductName { get; set; }
        public string StringProductType { get; set; }
        public string StringPackageType { get; set; }
        public string StringFactoryName { get; set; }
        public string StringTonnage { get; set; }
        public string StringProvince { get; set; }
        public string StringCity { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string StringInsertDateTime { get; set; }
        #region StringstartDateNews
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.StringstartDateNews)]
        #endregion StringstartDateNews
        public string StringstartDateNews { get; set; }
        #region StringEndDateNews
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.StringEndDateNews)]
        #endregion StringEndDateNews
        public string StringEndDateNews { get; set; }
        public Guid Id { get; set; }

        #region AmountPaid
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.AmountPaid)]
        #endregion
        public long AmountPaid { get; set; }
        public long AmountPaid1 { get; set; }

        #region DestinationAmountPaid
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.DestinationAmountPaid)]
        #endregion
        public long DestinationAmountPaid { get; set; }
        public string ref_id { get; set; }
        public string card_pan { get; set; }

        #region MahalTahvil
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.MahalTahvil)]
        #endregion
        public string MahalTahvil { get; set; }
        #region FinalApprove
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.FinalApprove)]
        #endregion

        public bool? FinalApprove { get; set; }
        #region stringFinalApprove
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.FinalApprove)]
        #endregion
        public string stringFinalApprove { get; set; }


        public Guid FinancialManagementId { get; set; }

    }
}
