using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.SubSystem
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.SubSystem),
           Name = Resources.ViewModel.Strings.SubSystemKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(30)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.SubSystem),
           Name = Resources.ViewModel.Strings.SubSystemKeys.Code)]
        //[System.ComponentModel.DataAnnotations.MaxLength(5)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public int Code { get; set; }

        #region UrlFrom
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.SubSystem),
           Name = Resources.ViewModel.Strings.SubSystemKeys.UrlFrom)]
    //    [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string UrlFrom { get; set; }

        #region UrlTo
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.SubSystem),
           Name = Resources.ViewModel.Strings.SubSystemKeys.UrlTo)]
  //      [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string UrlTo { get; set; }
    }
}
