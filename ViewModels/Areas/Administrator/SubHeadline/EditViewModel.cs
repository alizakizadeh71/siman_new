using System;

namespace ViewModels.Areas.Administrator.SubHeadLine
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

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
