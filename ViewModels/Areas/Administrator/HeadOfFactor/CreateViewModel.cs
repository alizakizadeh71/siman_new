using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.HeadOfFactor
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

        #region commodity
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

        #region CompanyName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CompanyName)]
        #endregion
        public string CompanyName { get; set; }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.Province)]
        #endregion
        public Guid Province { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.City)]
        #endregion
        public Guid? City { get; set; }

        #region CompanyNationalCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CompanyNationalCode)]
        #endregion
        public string CompanyNationalCode { get; set; }

        #region Description
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.Description)]
        #endregion
        public string Description { get; set; }

        #region CellPhoneNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CellPhoneNumber)]
        #endregion
        public string CellPhoneNumber { get; set; }
    }
}
