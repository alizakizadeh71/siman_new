namespace Models
{
    public class Paymentwaitinglist : BaseExtendedEntity
    {
        public Paymentwaitinglist() { }
        #region Inventorytonnage
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.ViewModel.Paymentwaitinglist),
            Name = Resources.ViewModel.Strings.PaymentwaitinglistKeys.Inventorytonnage)]
        #endregion
        public int Inventorytonnage { get; set; }
        #region ProductName
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.Paymentwaitinglist),
        Name = Resources.ViewModel.Strings.PaymentwaitinglistKeys.ProductName)]
        #endregion
        public virtual ProductName ProductName { get; set; }
        #region ProductName
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.Paymentwaitinglist),
        Name = Resources.ViewModel.Strings.PaymentwaitinglistKeys.ProductName)]
        #endregion
        public System.Guid ProductNameId { get; set; }
        #region ProductType
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.Paymentwaitinglist),
        Name = Resources.ViewModel.Strings.PaymentwaitinglistKeys.ProductType)]
        #endregion
        public virtual ProductType ProductType { get; set; }
        #region ProductType
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.Paymentwaitinglist),
        Name = Resources.ViewModel.Strings.PaymentwaitinglistKeys.ProductType)]
        #endregion
        public System.Guid ProductTypeId { get; set; }
        #region PackageType
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.Paymentwaitinglist),
        Name = Resources.ViewModel.Strings.PaymentwaitinglistKeys.PackageType)]
        #endregion
        public virtual PackageType PackageType { get; set; }
        #region PackageType
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.Paymentwaitinglist),
        Name = Resources.ViewModel.Strings.PaymentwaitinglistKeys.PackageType)]
        #endregion
        public System.Guid PackageTypeId { get; set; }
        #region FactoryName
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.Paymentwaitinglist),
        Name = Resources.ViewModel.Strings.PaymentwaitinglistKeys.FactoryName)]
        #endregion
        public virtual FactoryName FactoryName { get; set; }
        #region FactoryName
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.Paymentwaitinglist),
        Name = Resources.ViewModel.Strings.PaymentwaitinglistKeys.FactoryName)]
        #endregion
        public System.Guid FactoryNameId { get; set; }
        #region FinalAprrove
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.ViewModel.Paymentwaitinglist),
        Name = Resources.ViewModel.Strings.PaymentwaitinglistKeys.FinalAprrove)]
        #endregion
        public bool FinalAprrove { get; set; }
    }
}
