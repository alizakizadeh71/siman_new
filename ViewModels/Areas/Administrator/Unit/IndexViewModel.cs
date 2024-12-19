namespace ViewModels.Areas.Administrator.Unit
{
    public class IndexViewModel : System.Object
    {
        public IndexViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Unit),
           Name = Resources.ViewModel.Strings.UnitKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Description
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Unit),
           Name = Resources.ViewModel.Strings.UnitKeys.Description)]
        #endregion
        public string Description { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Unit),
           Name = Resources.ViewModel.Strings.UnitKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }
    }
}
