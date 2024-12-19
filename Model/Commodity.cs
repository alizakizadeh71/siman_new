namespace Models
{
    /// <summary>
    /// کالا
    /// </summary>
    public class Commodity : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Commodity>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(100);
            }
        }
        #endregion

        public Commodity()
        { }

        public string Name { get; set; }

        public int Code { get; set; }

        public virtual System.Collections.Generic.IList<CommodityInSubSystem> CommodityInSubSystems { get; set; }
    }
}
