namespace ViewModels.Areas.Administrator.IncomeRow
{
    public class IndexViewModel : System.Object
    {
        public IndexViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.IncomeRow),
           Name = Resources.ViewModel.Strings.IncomeRowKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.IncomeRow),
           Name = Resources.ViewModel.Strings.IncomeRowKeys.Code)]
        #endregion
        public string Code { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.IncomeRow),
           Name = Resources.ViewModel.Strings.IncomeRowKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }
    }
}
