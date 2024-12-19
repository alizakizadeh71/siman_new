namespace ViewModels.Areas.Administrator.BankAccount
{
    public class DetailViewModel : System.Object
    {
        public DetailViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Bank
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.Bank)]
        #endregion
        public string Bank { get; set; }

        #region ExecutableCode
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.ExecutableCode)]
        #endregion
        public string ExecutableCode { get; set; }

        #region IncomeRow
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.IncomeRow)]
        #endregion
        public string IncomeRow { get; set; }

        #region Certain
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.Certain)]
        #endregion
        public string Certain { get; set; }

        #region AccountTitel
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.AccountTitel)]
        #endregion
        public string AccountTitel { get; set; }

        #region AccountNumber
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.AccountNumber)]
        #endregion
        public string AccountNumber { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }

        #region UpdateDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.UpdateDateTime)]
        #endregion
        public string UpdateDateTime { get; set; }
    }
}
