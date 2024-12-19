namespace ViewModels.Areas.Administrator.SubHeadLine
{
    public class IndexViewModel : System.Object
    {
        public IndexViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.SubHeadLine),
           Name = Resources.Model.Strings.SubHeadLineKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.SubHeadLine),
           Name = Resources.Model.Strings.SubHeadLineKeys.Code)]
        #endregion
        public string Code { get; set; }

        #region HeadLine
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.SubHeadLine),
           Name = Resources.Model.Strings.SubHeadLineKeys.HeadLine)]
        #endregion
        public string HeadLine { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.SubHeadLine),
           Name = Resources.Model.Strings.SubHeadLineKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }

    }
}
