namespace ViewModels.Areas.Administrator.Certain
{
    public class IndexViewModel : System.Object
    {
        public IndexViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Certain),
           Name = Resources.ViewModel.Strings.CertainKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Certain),
           Name = Resources.ViewModel.Strings.CertainKeys.Code)]
        #endregion
        public string Code { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Certain),
           Name = Resources.ViewModel.Strings.CertainKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }
    }
}
