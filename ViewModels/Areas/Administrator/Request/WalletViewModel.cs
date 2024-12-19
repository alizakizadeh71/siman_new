using System;

namespace ViewModels.Areas.Administrator.Request
{
    public class WalletViewModel
    {
        public Guid Id { get; set; }
        public int? InvoiceNumber { get; set; }
        public string StringInsertDateTime { get; set; }
        public string BuyerMobile { get; set; }
        public string BuyerName { get; set; }
        public string ref_id { get; set; }
        public string card_pan { get; set; }
        public string AmountPaid { get; set; }
    }
}
