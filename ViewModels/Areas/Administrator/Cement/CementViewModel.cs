using System;
using System.Collections.Generic;
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

        #region ProductType
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.ProductType)]
        #endregion
        public Guid ProductType { get; set; }

        #region PackageType
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.PackageType)]
        #endregion
        public Guid PackageType { get; set; }

        #region FactoryName
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.FactoryName)]
        #endregion
        public Guid FactoryName { get; set; }

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

        #region City
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.City)]
        #endregion
        public Guid? City { get; set; }

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
        #endregion
        public string BuyerMobile { get; set; }

        #region Address
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.Address)]
        #endregion
        public string Address { get; set; }
    }
}
