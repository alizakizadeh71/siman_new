namespace Models
{
    /// <summary>
    /// کالا در زیر سیستم
    /// </summary>
    public class CommodityInSubSystem : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<CommodityInSubSystem>
        {
            public Configuration()
            {
                HasRequired(current => current.Commodity)
                    .WithMany(office => office.CommodityInSubSystems)
                    .HasForeignKey(current => current.CommodityId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.SubSystem)
                    .WithMany(office => office.CommodityInSubSystems)
                    .HasForeignKey(current => current.SubSystemId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.ServiceTariff)
                    .WithMany(office => office.CommodityInSubSystems)
                    .HasForeignKey(current => current.ServiceTariffId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion

        public CommodityInSubSystem()
        { }

        public virtual Commodity Commodity { get; set; }
        public System.Guid CommodityId { get; set; }

        public virtual SubSystem SubSystem { get; set; }
        public System.Guid SubSystemId { get; set; }

        public virtual ServiceTariff ServiceTariff { get; set; }
        public System.Guid ServiceTariffId { get; set; }
    }
}
