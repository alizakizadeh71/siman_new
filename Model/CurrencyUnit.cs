namespace Models
{
    public class CurrencyUnit : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<CurrencyUnit>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(30);
                Property(current => current.Ratio).HasPrecision(18, 3);


                HasRequired(current => current.User)
                    .WithMany(user => user.CurrencyUnits)
                    .HasForeignKey(current => current.UserId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion

        public CurrencyUnit()
        { }

        public string Name { get; set; }
        public string NameString
        {
            get
            {
                return this.Name + " - " + this.Ratio;
            }
        }

        public int Code { get; set; }

        public System.DateTime? ExpireDateTime { get; set; }

        public decimal Ratio { get; set; }

        public virtual User User { get; set; }

        public System.Guid UserId { get; set; }

        public string UserIPAddress { get; set; }
        public string Browser { get; set; }

        //public virtual System.Collections.Generic.IList<DetailOfFactor> DetailOfFactors { get; set; }
    }
}
