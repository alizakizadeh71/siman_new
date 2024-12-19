namespace Models
{
    public class Inventoryamount : BaseExtendedEntity
    {
        public Inventoryamount() { }
        #region Inventorytonnage
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.Inventorytonnage)]
        #endregion
        public int Inventorytonnage { get; set; }
        #region ProductName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.ProductName)]
        #endregion
        public virtual ProductName ProductName { get; set; }
        #region ProductName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.ProductName)]
        #endregion
        public System.Guid ProductNameId { get; set; }
        #region ProductType
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.ProductType)]
        #endregion
        public virtual ProductType ProductType { get; set; }
        #region ProductType
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.ProductType)]
        #endregion
        public System.Guid ProductTypeId { get; set; }
        #region PackageType
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.PackageType)]
        #endregion
        public virtual PackageType PackageType { get; set; }
        #region PackageType
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.PackageType)]
        #endregion
        public System.Guid PackageTypeId { get; set; }
        #region FactoryName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.FactoryName)]
        #endregion
        public virtual FactoryName FactoryName { get; set; }
        #region FactoryName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.FactoryName)]
        #endregion
        public System.Guid FactoryNameId { get; set; }
    }
}
