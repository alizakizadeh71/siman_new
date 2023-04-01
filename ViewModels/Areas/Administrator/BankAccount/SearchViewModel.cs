using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.BankAccount
{
    public class SearchViewModel : System.Object
    {
        public SearchViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Bank
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.Bank)]
        #endregion
        public Guid Bank { get; set; }

        #region ExecutableCode
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.ExecutableCode)]
        #endregion
        public Guid ExecutableCode { get; set; }

        #region IncomeRow
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.IncomeRow)]
        #endregion
        public Guid IncomeRow { get; set; }

        #region Certain
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.BankAccount),
           Name = Resources.ViewModel.Strings.BankAccountKeys.Certain)]
        #endregion
        public Guid Certain { get; set; }

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
    }
}
