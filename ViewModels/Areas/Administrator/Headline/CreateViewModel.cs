﻿namespace ViewModels.Areas.Administrator.HeadLine
{
    public class CreateViewModel : System.Object
    {
        public CreateViewModel()
        { }

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
        [System.ComponentModel.DataAnnotations.MaxLength(2)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Code { get; set; }
    }
}
