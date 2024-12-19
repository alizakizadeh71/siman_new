using System;

namespace ViewModels.Areas.Administrator.DetailOfFactor
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

        #region HeadOfFactor
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.DetailOfFactor),
             Name = Resources.Model.Strings.DetailOfFactorKeys.HeadOfFactor)]
        #endregion
        public Guid HeadOfFactor { get; set; }

        #region ServiceTariff
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.DetailOfFactor),
             Name = Resources.Model.Strings.DetailOfFactorKeys.ServiceTariff)]
        #endregion
        public Guid ServiceTariff { get; set; }

        #region CommodityDescription
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.CommodityDescription)]
        #endregion
        public string CommodityDescription { get; set; }

        #region CommodityCount
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.CommodityCount)]
        #endregion
        public int CommodityCount { get; set; }

        #region Description
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.Description)]
        #endregion
        public string Description { get; set; }

        #region Description
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CurrencyUnit)]
        #endregion
        public Guid? CurrencyUnit { get; set; }
    }
}
