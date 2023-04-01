namespace ViewModels.Account
{
	public class ChangePasswordViewModel : System.Object
	{
		public ChangePasswordViewModel()
		{
		}

        #region CurrentPassword
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.ChangePassword),
            Name = Resources.ViewModel.Strings.ChangePasswordKeys.CurrentPassword)]

        [System.ComponentModel.DataAnnotations.DataType
            (System.ComponentModel.DataAnnotations.DataType.Password)]

        [System.ComponentModel.DataAnnotations.Required
            (AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(Resources.Message.Global),
            ErrorMessageResourceName = Resources.Message.Strings.GlobalKeys.Required)]
        #endregion
		public string CurrentPassword { get; set; }

        #region NewPassword
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.ChangePassword),
            Name = Resources.ViewModel.Strings.ChangePasswordKeys.NewPassword)]

        [System.ComponentModel.DataAnnotations.DataType
            (System.ComponentModel.DataAnnotations.DataType.Password)]

        [System.ComponentModel.DataAnnotations.Required
            (AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(Resources.Message.Global),
            ErrorMessageResourceName = Resources.Message.Strings.GlobalKeys.Required)]
        #endregion
		public string NewPassword { get; set; }

        #region ConfirmNewPassword
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.ChangePassword),
            Name = Resources.ViewModel.Strings.ChangePasswordKeys.ConfirmNewPassword)]

        [System.ComponentModel.DataAnnotations.DataType
            (System.ComponentModel.DataAnnotations.DataType.Password)]

        [System.ComponentModel.DataAnnotations.Required
            (AllowEmptyStrings = false,
            ErrorMessageResourceType = typeof(Resources.Message.Global),
            ErrorMessageResourceName = Resources.Message.Strings.GlobalKeys.Required)]
        #endregion
		public string ConfirmNewPassword { get; set; }
	}
}
