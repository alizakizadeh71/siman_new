using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.SubHeadLine
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.SubHeadLine),
           Name = Resources.Model.Strings.SubHeadLineKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(400)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.SubHeadLine),
           Name = Resources.Model.Strings.SubHeadLineKeys.Code)]
        [System.ComponentModel.DataAnnotations.MaxLength(2)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Code { get; set; }


        #region HeadLine
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.SubHeadLine),
            Name = Resources.Model.Strings.SubHeadLineKeys.HeadLine)]
        #endregion
        public Guid HeadLine { get; set; }
    }
}
