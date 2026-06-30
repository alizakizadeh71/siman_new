using System;

namespace ViewModels.Areas.Administrator.Inventoryamount
{
    public class InventoryViewModel
    {
        public Guid ProductNameId { get; set; }
        public Guid ProductTypeId { get; set; }
        public Guid PackageType { get; set; }
        public Guid FactoryNameId { get; set; }
    }
}
