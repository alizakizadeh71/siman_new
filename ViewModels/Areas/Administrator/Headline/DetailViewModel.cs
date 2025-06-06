﻿namespace ViewModels.Areas.Administrator.HeadLine
{
    public class DetailViewModel : System.Object
    {
        public DetailViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.HeadLine),
           Name = Resources.Model.Strings.HeadLineKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.HeadLine),
           Name = Resources.Model.Strings.HeadLineKeys.Code)]
        #endregion
        public string Code { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.HeadLine),
           Name = Resources.Model.Strings.HeadLineKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }
    }
}
