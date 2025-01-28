namespace ViewModels.Areas.Administrator.Bank
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Bank),
           Name = Resources.ViewModel.Strings.BankKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Bank),
           Name = Resources.ViewModel.Strings.BankKeys.Balance)]
        #endregion
        public int Balance { get; set; }
    }
}
