namespace ViewModels.Areas.Administrator.BankAccount
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }


        #region Bank
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.Bank)]
        #endregion
        public System.Guid Bank { get; set; }

        #region ExecutableCode
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.ExecutableCode)]
        #endregion
        public System.Guid ExecutableCode { get; set; }

        #region IncomeRow
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.IncomeRow)]
        #endregion
        public System.Guid IncomeRow { get; set; }

        #region Certain
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.Certain)]
        #endregion
        public System.Guid Certain { get; set; }

        #region AccountTitel
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.AccountTitel)]
        [System.ComponentModel.DataAnnotations.MaxLength(200)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string AccountTitel { get; set; }

        #region AccountNumber
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.AccountNumber)]
        [System.ComponentModel.DataAnnotations.MaxLength(40)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string AccountNumber { get; set; }
    }
}
