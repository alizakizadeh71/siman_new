using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Administrator.Village
{
   public class IndexViewModel : System.Object
    {
        public IndexViewModel()
        { }

        public Guid Id { get; set; }


        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.City),
        Name = Resources.Model.Strings.villageKeys.Province)]
        public Guid Province { get; set; }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.village),
        Name = Resources.Model.Strings.villageKeys.City)]

        public Guid City { get; set; }
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.village),
        Name = Resources.Model.Strings.villageKeys.stringCity)]
        public string stringCity { get; set; }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.village),
        Name = Resources.Model.Strings.villageKeys.Name)]

        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.village),
        Name = Resources.Model.Strings.villageKeys.Code)]
        public string Code { get; set; }

        public string StringInsertDateTime { get; set; }
    }
}
