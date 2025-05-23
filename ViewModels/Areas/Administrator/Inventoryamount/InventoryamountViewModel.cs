using System;

namespace ViewModels.Areas.Administrator.Inventoryamount
{
    public class InventoryamountViewModel : System.Object
    {
        public InventoryamountViewModel()
        { }

        public Guid Id { get; set; }
        #region Inventorytonnage
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.Inventorytonnage)]
        #endregion
        public double Inventorytonnage { get; set; }
        #region ProductName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.ProductName)]
        #endregion
        public Guid ProductName { get; set; }
        //#region ProductName
        //[System.ComponentModel.DataAnnotations.Display
        //    (ResourceType = typeof(Resources.Model.Inventoryamount),
        //    Name = Resources.Model.Strings.InventoryamountKeys.ProductName)]
        //#endregion
        public string stringProductName { get; set; }
        #region ProductType
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.ProductType)]
        #endregion
        public Guid ProductType { get; set; }
        //#region ProductType
        //[System.ComponentModel.DataAnnotations.Display
        //    (ResourceType = typeof(Resources.Model.Inventoryamount),
        //    Name = Resources.Model.Strings.InventoryamountKeys.ProductType)]
        //#endregion
        public string stringProductType { get; set; }
        #region PackageType
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.PackageType)]
        #endregion
        public Guid PackageType { get; set; }
        //#region PackageType
        //[System.ComponentModel.DataAnnotations.Display
        //    (ResourceType = typeof(Resources.Model.Inventoryamount),
        //    Name = Resources.Model.Strings.InventoryamountKeys.PackageType)]
        //#endregion
        public string stringPackageType { get; set; }
        #region FactoryName
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Inventoryamount),
            Name = Resources.Model.Strings.InventoryamountKeys.FactoryName)]
        #endregion
        public Guid FactoryName { get; set; }
        //#region FactoryName
        //[System.ComponentModel.DataAnnotations.Display
        //    (ResourceType = typeof(Resources.Model.Inventoryamount),
        //    Name = Resources.Model.Strings.InventoryamountKeys.FactoryName)]
        //#endregion
        public string stringFactoryName { get; set; }
    }
}
