namespace ViewModels.Areas.Administrator.AccountNumberManage
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

        #region SubSystem
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.AccountNumberManage),
           Name = Resources.ViewModel.Strings.AccountNumberManageKeys.SubSystem)]
        #endregion
        public System.Guid SubSystem { get; set; }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.AccountNumberManage),
           Name = Resources.ViewModel.Strings.AccountNumberManageKeys.Province)]
        #endregion
        public System.Guid Province { get; set; }

        #region AccountNumber
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.AccountNumberManage),
           Name = Resources.ViewModel.Strings.AccountNumberManageKeys.AccountNumber)]
        #endregion
        public System.Guid AccountNumber { get; set; }

        public string BaseAccountNumber { get; set; }
    }
}
