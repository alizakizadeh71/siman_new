using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.CurrencyUnit
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
           Name = Resources.ViewModel.Strings.CurrencyUnitKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(30)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }


        //#region Code
        //[System.ComponentModel.DataAnnotations.Display
        //   (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
        //   Name = Resources.ViewModel.Strings.CurrencyUnitKeys.Code)]
        //[System.ComponentModel.DataAnnotations.MaxLength(5)]
        //[System.ComponentModel.DataAnnotations.Required]
        //#endregion
        //public string Code { get; set; }


        #region Ratio
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
           Name = Resources.ViewModel.Strings.CurrencyUnitKeys.Ratio)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public decimal Ratio { get; set; }

        #region ExpireDateTime
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
            Name = Resources.ViewModel.Strings.CurrencyUnitKeys.ExpireDateTime)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public System.DateTime? ExpireDateTime { get; set; }

    }
}
