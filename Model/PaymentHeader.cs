namespace Models
{
    public class PaymentHeader : BaseExtendedEntity
    {
        internal class Configuration :
            System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PaymentHeader>
        {
            public Configuration()
            {
                ToTable("PaymentHeader");
                //Property(current => current.Titele).HasMaxLength(200);
            }
        }
        public string Titele { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Date { get; set; }
        public virtual System.Collections.Generic.IList<PaymentDetail> PaymentDetails { get; set; }

    }
}
