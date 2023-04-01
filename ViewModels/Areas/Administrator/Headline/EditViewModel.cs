using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.HeadLine
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.HeadLine),
           Name = Resources.Model.Strings.HeadLineKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(300)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.HeadLine),
           Name = Resources.Model.Strings.HeadLineKeys.Code)]
        [System.ComponentModel.DataAnnotations.MaxLength(1)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Code { get; set; }
    }
}
