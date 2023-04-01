using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.AccountNumberManage
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

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

    }
}
