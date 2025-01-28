namespace ViewModels.Areas.Administrator.Bank
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Bank),
           Name = Resources.ViewModel.Strings.BankKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region Balance
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Bank),
           Name = Resources.ViewModel.Strings.BankKeys.Balance)]
        #endregion
        public int Balance { get; set; }
    }
}
