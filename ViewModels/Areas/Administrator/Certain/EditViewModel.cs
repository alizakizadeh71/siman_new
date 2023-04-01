using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.Certain
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Certain),
           Name = Resources.ViewModel.Strings.CertainKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Certain),
           Name = Resources.ViewModel.Strings.CertainKeys.Code)]
        [System.ComponentModel.DataAnnotations.MaxLength(2)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Code { get; set; }
    }
}
