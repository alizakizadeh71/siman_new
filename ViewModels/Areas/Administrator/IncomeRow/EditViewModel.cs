﻿namespace ViewModels.Areas.Administrator.IncomeRow
{
    public class EditViewModel : System.Object
    {
        public EditViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.IncomeRow),
           Name = Resources.ViewModel.Strings.IncomeRowKeys.Name)]
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.IncomeRow),
           Name = Resources.ViewModel.Strings.IncomeRowKeys.Code)]
        [System.ComponentModel.DataAnnotations.MaxLength(6)]
        [System.ComponentModel.DataAnnotations.Required]
        #endregion
        public string Code { get; set; }
    }
}
