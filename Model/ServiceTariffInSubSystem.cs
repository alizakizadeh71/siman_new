namespace Models
{
    /// <summary>
    /// ارتباط زیر سیستم در تعرفه
    /// </summary>
    public class ServiceTariffInSubSystem : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ServiceTariffInSubSystem>
        {
            public Configuration()
            {
                HasRequired(current => current.SubSystem)
                    .WithMany(office => office.ServiceTariffInSubSystems)
                    .HasForeignKey(current => current.SubSystemId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.ServiceTariff)
                    .WithMany(office => office.ServiceTariffInSubSystems)
                    .HasForeignKey(current => current.ServiceTariffId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion

        public ServiceTariffInSubSystem()
        { }

        public virtual SubSystem SubSystem { get; set; }
        public System.Guid SubSystemId { get; set; }

        public virtual ServiceTariff ServiceTariff { get; set; }
        public System.Guid ServiceTariffId { get; set; }
    }
}
