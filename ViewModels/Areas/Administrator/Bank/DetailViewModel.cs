﻿namespace ViewModels.Areas.Administrator.Bank
{
    public class DetailViewModel : System.Object
    {
        public DetailViewModel()
        { }

        public System.Guid Id { get; set; }

        #region Name
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Bank),
           Name = Resources.ViewModel.Strings.BankKeys.Name)]
        #endregion
        public string Name { get; set; }

        #region Code
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Bank),
           Name = Resources.ViewModel.Strings.BankKeys.Code)]
        #endregion
        public string Code { get; set; }

        #region InsertDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.ViewModel.Bank),
           Name = Resources.ViewModel.Strings.BankKeys.InsertDateTime)]
        #endregion
        public string InsertDateTime { get; set; }
    }
}
