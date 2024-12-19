namespace ViewModels.Areas.Administrator.DetailOfFactor
{
    public class IndexViewModel : System.Object
    {
        public IndexViewModel()
        { }

        public System.Guid Id { get; set; }

        #region HeadOfFactor
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.DetailOfFactor),
             Name = Resources.Model.Strings.DetailOfFactorKeys.HeadOfFactor)]
        #endregion
        public string HeadOfFactor { get; set; }

        #region ServiceTariff
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.DetailOfFactor),
             Name = Resources.Model.Strings.DetailOfFactorKeys.ServiceTariff)]
        #endregion
        public string ServiceTariff { get; set; }

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

        #region Price per unit
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.PricePerUnit)]
        #endregion
        public decimal? PricePerUnit { get; set; }

        #region Total price
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.DetailOfFactor),
            Name = Resources.Model.Strings.DetailOfFactorKeys.TotalPrice)]
        #endregion
        public decimal? TotalPrice { get; set; }

        #region HeadLineId
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.HeadOfFactor),
             Name = Resources.Model.Strings.HeadOfFactorKeys.HeadLine)]
        #endregion
        public string HeadLine { get; set; }

        #region SubHeadLineId
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.HeadOfFactor),
             Name = Resources.Model.Strings.HeadOfFactorKeys.SubHeadLine)]
        #endregion
        public string SubHeadLine { get; set; }

        #region CompanyName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CompanyName)]
        #endregion
        public string CompanyName { get; set; }
    }
}
