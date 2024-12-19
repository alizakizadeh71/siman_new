using System;

namespace ViewModels.Areas.Administrator.CurrencyUnit
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
           Name = Resources.ViewModel.Strings.CurrencyUnitKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Ratio
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
           Name = Resources.ViewModel.Strings.CurrencyUnitKeys.Ratio)]
        #endregion
        public decimal Ratio { get; set; }

        #region ExpireDateTime
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.CurrencyUnit),
            Name = Resources.ViewModel.Strings.CurrencyUnitKeys.ExpireDateTime)]
        #endregion
        public DateTime? ExpireDateTime { get; set; }

    }
}
