namespace Models
{
    /// <summary>
    /// واحد
    /// </summary>
    public class Unit : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Unit>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(30);
                Property(current => current.Description).HasMaxLength(100);
            }
        }
        #endregion

        public Unit()
        { }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual System.Collections.Generic.IList<ServiceTariff> ServiceTariffs { get; set; }
    }
}
