namespace ViewModels.Areas.Administrator.CarrierInventory
{
    public class CarrierInventoryViewModel
    {
        public System.Guid Id { get; set; }
        public System.Guid CarrierId { get; set; }
        public System.Guid ProductNameId { get; set; }
        public System.Guid ProductTypeId { get; set; }
        public System.Guid PackageTypeId { get; set; }
        public System.Guid FactoryNameId { get; set; }
        public double InventoryTonnage { get; set; }
        public bool IsDefaultCarrier { get; set; }

        // Display
        public string StringCarrierName { get; set; }
        public string StringProductName { get; set; }
        public string StringProductType { get; set; }
        public string StringPackageType { get; set; }
        public string StringFactoryName { get; set; }
        public string StringInventoryTonnage { get; set; }
        public string StringIsDefaultCarrier { get; set; }
        public string StringInsertDateTime { get; set; }
    }
}