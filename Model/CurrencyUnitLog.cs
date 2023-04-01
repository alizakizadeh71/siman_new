namespace Models
{
    public class CurrencyUnitLog : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<CurrencyUnitLog>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(30);
            }
        }
        #endregion

        public CurrencyUnitLog()
        { }

        public System.Guid OldId { get; set; }

        public string Name { get; set; }

        public int Code { get; set; }

        public System.DateTime? ExpireDateTime { get; set; }

        public System.DateTime? CancellationDateTime { get; set; }

        public decimal Ratio { get; set; }

        public System.Guid UserId { get; set; }

		public string UserIPAddress { get; set; }
		public string Browser { get; set; }
	}
}
