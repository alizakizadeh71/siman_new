using System;

namespace ViewModels.Areas.Administrator.User
{
    public class AuthenticateViewModel : System.Object
    {
        public AuthenticateViewModel() { }

        #region NationalCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.NationalCode)]
        #endregion
        public string NationalCode { get; set; }
        public string NationalCode2 { get; set; }

        #region BirthDay
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.BirthDay)]
        #endregion
        public DateTime? BirthDay { get; set; }
        public DateTime? BirthDay2 { get; set; }

        #region FullName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.FullName)]
        #endregion
        public string FullName { get; set; }

        #region PostalCode
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.PostalCode)]
        #endregion
        public string PostalCode { get; set; }
        public string PostalCode2 { get; set; }

        #region Address
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Address)]
        #endregion
        public string Address { get; set; }  // آدرس شامل فیلد های استان - شهر - محله - خیابان اصلی - خیابان فرعی - کوچه - پلاک - طبقه

        #region Image
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Image)]
        #endregion
        public string Image { get; set; }

        #region IdentityCertificateSerial
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.IdentityCertificateSerial)]
        #endregion
        public string IdentityCertificateSerial { get; set; }
        public string IdentityCertificateSerial2 { get; set; }

    }
}
