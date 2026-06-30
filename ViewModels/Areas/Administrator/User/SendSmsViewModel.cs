using System.Collections.Generic;

namespace ViewModels.Areas.Administrator.User
{
    public class SendSmsViewModel
    {
        public string Text { get; set; }
        public List<UserSmsSelectionViewModel> Users { get; set; }
        public string SearchTerm { get; set; }
    }


}
