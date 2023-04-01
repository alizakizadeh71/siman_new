using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.Certain
{
    public class DetailViewModel : System.Object
    {
        public DetailViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Certain),
           Name = Resources.ViewModel.Strings.CertainKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Certain),
           Name = Resources.ViewModel.Strings.CertainKeys.Code)]
        #endregion
        public string Code { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Certain),
           Name = Resources.ViewModel.Strings.CertainKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }
    }
}
