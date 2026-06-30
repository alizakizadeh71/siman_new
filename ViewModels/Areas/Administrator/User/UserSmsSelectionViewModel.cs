using System;

namespace ViewModels.Areas.Administrator.User
{
    public class UserSmsSelectionViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsSelected { get; set; }
    }
}
