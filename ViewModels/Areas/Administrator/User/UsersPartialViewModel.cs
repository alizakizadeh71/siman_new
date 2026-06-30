using PagedList;
using System.Collections.Generic;

namespace ViewModels.Areas.Administrator.User
{
    public class UsersPartialViewModel
    {
        public List<UserSmsSelectionViewModel> Users { get; set; }          // برای POST
        public IPagedList<UserSmsSelectionViewModel> PagedUsers { get; set; } // برای نمایش
    }
}
