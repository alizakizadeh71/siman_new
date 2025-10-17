using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace ViewModels.Areas.Administrator.User
{
    public class SendSmsViewModel
    {
        public string Text { get; set; }
        public List<UserSmsSelectionViewModel> Users { get; set; }
        public string SearchTerm { get; set; }
    }


}
