using System;
using System.Linq;
using System.Data.Entity;

namespace ViewModels.Areas.Administrator.User
{
    public class SearchViewModel : System.Object
    {
        public SearchViewModel()
        { }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Province)]
        #endregion
        public Guid Province { get; set; }
        
        #region City
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.City)]
        #endregion
        public Guid City { get; set; }

        #region Role
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.Role)]
        #endregion
        public Guid Role { get; set; }

        #region FullName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.FullName)]
        #endregion
        public string FullName { get; set; }

        #region UserName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.User),
            Name = Resources.ViewModel.Strings.UserKeys.UserName)]
        #endregion
        public string UserName { get; set; }
    }
}
