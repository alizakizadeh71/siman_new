using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Bank),
           Name = Resources.ViewModel.Strings.BankKeys.Code)]
        [System.ComponentModel.DataAnnotations.MaxLength(1)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Code { get; set; }
    }
}
