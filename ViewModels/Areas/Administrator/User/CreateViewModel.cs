using System;

namespace ViewModels.Areas.Administrator.User
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Province)]
        #endregion
        public Guid Province { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.City)]
        #endregion
        public Guid City { get; set; }

        #region Role
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Role)]
        #endregion
        public Guid Role { get; set; }

        #region NationalCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.NationalCode)]
        #endregion
        public string NationalCode { get; set; }
        #region creditAmount
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.creditAmount)]
        #endregion
        public int creditAmount { get; set; }

        #region BirthDay
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.BirthDay)]
        #endregion
        public DateTime? BirthDay { get; set; }

        #region FullName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.FullName)]
        #endregion
        public string FullName { get; set; }

        #region UserName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.UserName)]
        #endregion
        public string UserName { get; set; }

        #region Password
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Password)]
        #endregion
        public string Password { get; set; }
        #region InitialCredit
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.InitialCredit)]
        #endregion
        public int InitialCredit { get; set; }

        #region IsApprovallicense
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.IsApprovallicense)]
        #endregion
        public bool IsApprovallicense { get; set; }

        #region isSendSmS
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.isSendSmS)]
        #endregion
        public bool isSendSmS { get; set; }

        #region IsMarketer
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.IsMarketer)]
        #endregion
        public bool IsMarketer { get; set; }
        #region MarketingCode
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.MarketingCode)]
        #endregion
        public string MarketingCode { get; set; }
        #region ReferredByCode
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.ReferredByCode)]
        #endregion
        public string ReferredByCode { get; set; }
    }
}
