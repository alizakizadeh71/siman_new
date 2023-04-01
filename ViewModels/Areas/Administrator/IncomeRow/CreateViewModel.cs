using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.IncomeRow
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.IncomeRow),
           Name = Resources.ViewModel.Strings.IncomeRowKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.IncomeRow),
           Name = Resources.ViewModel.Strings.IncomeRowKeys.Code)]
        [System.ComponentModel.DataAnnotations.MaxLength(6)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Code { get; set; }
    }
}
