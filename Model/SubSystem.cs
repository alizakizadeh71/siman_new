namespace Models
{
    public class SubSystem : BaseExtendedEntity
    {
        #region Configuration
        internal class Configuration :
            System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SubSystem>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(100);
                Property(current => current.UrlFrom).HasMaxLength(500);
                Property(current => current.UrlTo).HasMaxLength(500);
            }
        }
        #endregion /Configuration

        public SubSystem()
        {
        }

        public string Name { get; set; }

        public int Code { get; set; }

        public string UrlFrom { get; set; }

        public string UrlTo { get; set; }

        public virtual System.Collections.Generic.IList<AccountNumberManage> AccountNumberManages { get; set; }
        public virtual System.Collections.Generic.IList<Request> Requests { get; set; }
        public virtual System.Collections.Generic.IList<HeadOfFactor> HeadOfFactors { get; set; }
        public virtual System.Collections.Generic.IList<CommodityInSubSystem> CommodityInSubSystems { get; set; }
        public virtual System.Collections.Generic.IList<ServiceTariffInSubSystem> ServiceTariffInSubSystems { get; set; }
    }
}
