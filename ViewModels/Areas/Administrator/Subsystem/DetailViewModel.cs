using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.SubSystem
{
    public class DetailViewModel : System.Object
    {
        public DetailViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.SubSystem),
           Name = Resources.ViewModel.Strings.SubSystemKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.SubSystem),
           Name = Resources.ViewModel.Strings.SubSystemKeys.Code)]
        #endregion
        public int Code { get; set; }

        #region UrlFrom
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.SubSystem),
           Name = Resources.ViewModel.Strings.SubSystemKeys.UrlFrom)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string UrlFrom { get; set; }

        #region UrlTo
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.SubSystem),
           Name = Resources.ViewModel.Strings.SubSystemKeys.UrlTo)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string UrlTo { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.SubSystem),
           Name = Resources.ViewModel.Strings.SubSystemKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }

        #region ExpireDateTime
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.SubSystem),
            Name = Resources.ViewModel.Strings.SubSystemKeys.UpdateDateTime)]
        #endregion
        public string UpdateDateTime { get; set; }

    }
}
