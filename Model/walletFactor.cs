using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class walletFactor : BaseExtendedEntity
    {
        public walletFactor()
        {
        }
        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<walletFactor>
        {
            public Configuration()
            {
                HasRequired(current => current.User)
                .WithMany(user => user.walletFactor)
                .HasForeignKey(current => current.UserId)
                .WillCascadeOnDelete(false)
                ;
            }
        }

        public virtual User User { get; set; }
        public System.Guid UserId { get; set; }
        public int Chargeamount { get; set; }
        public string URLAddress { get; set; }
        public string UserIPAddress { get; set; }
        public string Browser { get; set; }
        public bool FinalApprove { get; set; }
        public string Authority { get; set; }
        public int? InvoiceNumber { get; set; }
        public string BuyerMobile { get; set; }
        public int Bankcode { get; set; }
        public string card_pan { get; set; }
        public long ref_id { get; set; }
        public System.DateTime? AmountPaidDate { get; set; }
    }
}
