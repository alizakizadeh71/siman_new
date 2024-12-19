namespace ViewModels.Areas.Administrator.ExecutableCode
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.ExecutableCode),
           Name = Resources.ViewModel.Strings.ExecutableCodeKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.ExecutableCode),
           Name = Resources.ViewModel.Strings.ExecutableCodeKeys.Code)]
        [System.ComponentModel.DataAnnotations.MaxLength(4)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Code { get; set; }
    }
}
