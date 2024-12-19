
namespace ViewModels.Account
{
    public class LoginViewModel : System.Object
    {
        public LoginViewModel()
        {
        }
        #region UserName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.User),
            Name = Resources.Model.Strings.UserKeys.UserName)]

        [System.ComponentModel.DataAnnotations.Required
            (AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(Resources.Message.Global),
            ErrorMessageResourceName = Resources.Message.Strings.GlobalKeys.Required)]
        #endregion
        public string UserName { get; set; }

        #region Password
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.User),
            Name = Resources.Model.Strings.UserKeys.Password)]

        [System.ComponentModel.DataAnnotations.DataType
            (System.ComponentModel.DataAnnotations.DataType.Password)]

        [System.ComponentModel.DataAnnotations.Required
            (AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(Resources.Message.Global),
            ErrorMessageResourceName = Resources.Message.Strings.GlobalKeys.Required)]
        #endregion
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [System.ComponentModel.DataAnnotations.Display(Name = "حاصل جمع")]
        public string Captcha { get; set; }
    }
}
