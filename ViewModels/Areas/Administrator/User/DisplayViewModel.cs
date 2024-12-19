using System;

namespace ViewModels.Areas.Administrator.User
{
    public class DisplayViewModel : System.Object
    {
        public DisplayViewModel()
        { }

        public System.Guid Id { get; set; }

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

        #region Role
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Role)]
        #endregion
        public string Role { get; set; }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Province)]
        #endregion
        public string Province { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.City)]
        #endregion
        public string City { get; set; }

        #region IsActive
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.IsActive)]
        #endregion
        public bool IsActive { get; set; }
        public string Active { get; set; }

        #region IsApprovallicense
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.IsApprovallicense)]
        #endregion
        public bool IsApprovallicense { get; set; }
        public string Approvallicense { get; set; }

        #region Authenticate
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Authenticate)]
        #endregion
        public string Authenticate { get; set; }

        #region Image
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Image)]
        #endregion
        public string Image { get; set; }

        #region NationalCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.NationalCode)]
        #endregion
        public string NationalCode { get; set; }

        #region BirthDay
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.BirthDay)]
        #endregion
        public DateTime? BirthDay { get; set; }

        #region PostalCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.PostalCode)]
        #endregion
        public string PostalCode { get; set; }

        #region Address
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Address)]
        #endregion
        public string Address { get; set; }

        #region creditAmount
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.creditAmount)]
        #endregion
        public int creditAmount { get; set; }

        #region IdentityCertificateSerial
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.IdentityCertificateSerial)]
        #endregion
        public string IdentityCertificateSerial { get; set; }
        #region InitialCredit
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.InitialCredit)]
        #endregion
        public int InitialCredit { get; set; }

    }
}
