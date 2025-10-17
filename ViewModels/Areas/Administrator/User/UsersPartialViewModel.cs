using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace ViewModels.Areas.Administrator.User
{
    public class UsersPartialViewModel
    {
        public List<UserSmsSelectionViewModel> Users { get; set; }          // برای POST
        public IPagedList<UserSmsSelectionViewModel> PagedUsers { get; set; } // برای نمایش
    }
}
