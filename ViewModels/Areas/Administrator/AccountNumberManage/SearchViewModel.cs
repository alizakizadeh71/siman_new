using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.AccountNumberManage
{
    public class SearchViewModel : System.Object
    {
        public SearchViewModel()
        { }

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

    }
}
