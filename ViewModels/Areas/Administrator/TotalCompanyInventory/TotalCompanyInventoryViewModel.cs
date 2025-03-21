using System;

namespace ViewModels.Areas.Administrator.TotalCompanyInventory
{
    public class TotalCompanyInventoryViewModel : System.Object
    {
        public TotalCompanyInventoryViewModel()
        { }
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.TotalCompanyInventory),
            Name = Resources.Model.Strings.TotalCompanyInventoryKeys.PriceInventory)]
        public string PriceInventory { get; set; }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.TotalCompanyInventory),
            Name = Resources.Model.Strings.TotalCompanyInventoryKeys.InventorytonnageString)]
        public string InventorytonnageString { get; set; }

        public long InventoryUserss { get; set; }
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.TotalCompanyInventory),
            Name = Resources.Model.Strings.TotalCompanyInventoryKeys.InventoryUsersString)]
        public string InventoryUsersString { get; set; }
        public Guid UserId { get; set; }
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.TotalCompanyInventory),
            Name = Resources.Model.Strings.TotalCompanyInventoryKeys.TotalBankamount)]
        public string TotalBankamount { get; set; }
    }
}
