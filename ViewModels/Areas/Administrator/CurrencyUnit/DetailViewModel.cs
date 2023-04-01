using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.CurrencyUnit
{
    public class DetailViewModel : System.Object
    {
        public DetailViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
           Name = Resources.ViewModel.Strings.CurrencyUnitKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
           Name = Resources.ViewModel.Strings.CurrencyUnitKeys.Code)]
        #endregion
        public string Code { get; set; }

        #region Ratio
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
           Name = Resources.ViewModel.Strings.CurrencyUnitKeys.Ratio)]
        #endregion
        public decimal Ratio { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
           Name = Resources.ViewModel.Strings.CurrencyUnitKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }

        #region ExpireDateTime
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
            Name = Resources.ViewModel.Strings.CurrencyUnitKeys.ExpireDateTime)]
        #endregion
        public string ExpireDateTime { get; set; }

    }
}
