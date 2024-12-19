namespace ViewModels.Areas.Administrator.AccountNumberManage
{
    public class DetailViewModel : System.Object
    {
        public DetailViewModel()
        { }

        public System.Guid Id { get; set; }

        #region SubSystem
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.AccountNumberManage),
           Name = Resources.ViewModel.Strings.AccountNumberManageKeys.SubSystem)]
        #endregion
        public string SubSystem { get; set; }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.AccountNumberManage),
           Name = Resources.ViewModel.Strings.AccountNumberManageKeys.Province)]
        #endregion
        public string Province { get; set; }

        #region AccountNumber
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.AccountNumberManage),
           Name = Resources.ViewModel.Strings.AccountNumberManageKeys.AccountNumber)]
        #endregion
        public string AccountNumber { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.AccountNumberManage),
           Name = Resources.ViewModel.Strings.AccountNumberManageKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }

        #region UpdateDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.AccountNumberManage),
           Name = Resources.ViewModel.Strings.AccountNumberManageKeys.UpdateDateTime)]
        #endregion
        public string UpdateDateTime { get; set; }
    }
}
