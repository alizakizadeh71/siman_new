using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PaymentDetail : BaseExtendedEntity
    {
        internal class Configuration :
            System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PaymentDetail>
        {
            public Configuration()
            {
                ToTable("PaymentDetail");

                HasRequired(current => current.PaymentHeader)
                    .WithMany(user => user.PaymentDetails)
                    .HasForeignKey(current => current.PaymentHeaderId)
                    .WillCascadeOnDelete(false);

                //Property(current => current.Titele).HasMaxLength(200);
            }
        }


        public virtual PaymentHeader PaymentHeader { get; set; }
        public System.Guid PaymentHeaderId { get; set; }

        public string TransactionNumber { get; set; }
        public double Value { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionAccountType { get; set; }
        public double Remind { get; set; }
        public string Device { get; set; }
        public DateTime DateDone { get; set; }
        public string Description { get; set; }
        public string PaymentCode { get; set; }
        public string PaymentIdentifier { get; set; }

    }
}
