using System;
using System.Linq;
using System.Data.Entity;

namespace ViewModels.Areas.Administrator.HeadOfFactor
{
    public class SearchViewModel : System.Object
    {
        public SearchViewModel()
        { }

        #region CompanyName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CompanyName)]
        #endregion
        public string CompanyName { get; set; }

        #region CompanyNationalCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CompanyNationalCode)]
        #endregion
        public string CompanyNationalCode { get; set; }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.Province)]
        #endregion
        public System.Guid? Province { get; set; }

        #region HeadLineId
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.HeadOfFactor),
             Name = Resources.Model.Strings.HeadOfFactorKeys.HeadLine)]
        #endregion
        public Guid HeadLine { get; set; }

        #region SubHeadLineId
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.HeadOfFactor),
             Name = Resources.Model.Strings.HeadOfFactorKeys.SubHeadLine)]
        #endregion
        public Guid SubHeadLine { get; set; }


        #region InvoiceNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.InvoiceNumber)]
        #endregion
        public int? InvoiceNumber { get; set; }

        #region StartDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.StartDate)]
        #endregion
        public DateTime? StartDate { get; set; }

        #region EndDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.EndDate)]
        #endregion
        public DateTime? EndDate { get; set; }

    }
}
