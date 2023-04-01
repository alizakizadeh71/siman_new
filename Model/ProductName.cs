namespace Models
{
    /// <summary>
    /// نام کالا 
    /// </summary>
    public class ProductName : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProductName>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(30);
                Property(current => current.Code).HasMaxLength(2);
            }
        }
        #endregion

        public ProductName()
        { }
        
        public string Name { get; set; }

        public string Code { get; set; }

        public virtual System.Collections.Generic.IList<ProductType> ProductTypes { get; set; }
    }
}
