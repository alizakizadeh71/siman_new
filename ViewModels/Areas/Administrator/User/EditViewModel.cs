using System;

namespace ViewModels.Areas.Administrator.User
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

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
        public Guid? City { get; set; }

        #region Role
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Role)]
        #endregion
        public Guid Role { get; set; }

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

        #region IsActive
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.IsActive)]
        #endregion
        public bool IsActive { get; set; }

        #region IsApprovallicense
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.IsApprovallicense)]
        #endregion
        public bool IsApprovallicense { get; set; }

        #region Authenticate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Authenticate)]
        #endregion
        public bool Authenticate { get; set; }

        #region creditAmount
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.creditAmount)]
        #endregion
        public int creditAmount { get; set; }

        #region Image
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Image)]
        #endregion
        public string Image { get; set; }
        #region BuyerMobile
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.BuyerMobile)]
        #endregion
        public string BuyerMobile { get; set; }
        #region Address
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Address)]
        #endregion
        public string Address { get; set; }

        #region InitialCredit
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.InitialCredit)]
        #endregion
        public int InitialCredit { get; set; }

        //#region IsDelete
        //[System.ComponentModel.DataAnnotations.Display
        //    (ResourceType = typeof(Resources.ViewModel.User),
        //    Name = Resources.ViewModel.Strings.UserKeys.IsDelete)]
        //#endregion
        //public bool? IsDelete { get; set; }
    }
}
