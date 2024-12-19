namespace ViewModels.Areas.Administrator.HeadOfFactor
{
    public class IndexFinancialViewModel : System.Object
    {
        public IndexFinancialViewModel()
        { }

        public System.Guid Id { get; set; }

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

        #region Province
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.Province)]
        #endregion
        public string Province { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.City)]
        #endregion
        public string City { get; set; }

        #region CompanyNationalCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CompanyNationalCode)]
        #endregion
        public string CompanyNationalCode { get; set; }

        #region InvoiceNumber
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.InvoiceNumber)]
        #endregion
        public int InvoiceNumber { get; set; }

        #region InvoiceDate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.InvoiceDate)]
        #endregion
        public string InvoiceDate { get; set; }

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
