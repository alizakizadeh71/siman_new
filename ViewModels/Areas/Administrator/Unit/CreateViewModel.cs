using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.Unit
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Unit),
           Name = Resources.ViewModel.Strings.UnitKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(30)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region Description
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Unit),
           Name = Resources.ViewModel.Strings.UnitKeys.Description)]
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        #endregion
        public string Description { get; set; }
    }
}
