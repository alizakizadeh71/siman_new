using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;


namespace ViewModels
{
    public interface IComboboxItem
    {
        string Name { get; }

        int Id { get; }
    }

    public class ComboboxItem : IComboboxItem
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }




    public interface IComboboxItemGuid
    {
        string Name { get; }

        System.Guid Id { get; }
    }

    public class ComboboxItemGuid : IComboboxItemGuid
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }
        public string MName { get; set; }

    }


    public interface IEnumComboboxItem
    {
        string Name { get; }

        int Id { get; }
    }

    public class EnumComboboxItem : IEnumComboboxItem
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
