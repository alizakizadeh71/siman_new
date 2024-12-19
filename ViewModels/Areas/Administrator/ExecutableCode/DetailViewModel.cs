namespace ViewModels.Areas.Administrator.ExecutableCode
{
    public class DetailViewModel : System.Object
    {
        public DetailViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.ExecutableCode),
           Name = Resources.ViewModel.Strings.ExecutableCodeKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.ExecutableCode),
           Name = Resources.ViewModel.Strings.ExecutableCodeKeys.Code)]
        #endregion
        public string Code { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.ExecutableCode),
           Name = Resources.ViewModel.Strings.ExecutableCodeKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }
    }
}
