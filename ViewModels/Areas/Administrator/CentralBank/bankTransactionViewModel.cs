namespace ViewModels.Areas.Administrator.CentralBank
{
    public class bankTransactionViewModel : System.Object
    {

        //public string TransactionNumber;
        //public double Value;
        //public object TransactionAccountType;
        //public double Remind;
        //public string Device;
        //public DateTime DateDone;
        //public string Description;
        //public string PaymentCode;

        public bankTransactionViewModel()
        { }
        public System.Guid Id { get; set; }

        #region AccountNumber
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.CentralBank),
           Name = Resources.Model.Strings.CentralBankKeys.AccountNumber)]
        #endregion
        public string AccountNumber { get; set; }

        #region FromDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.CentralBank),
           Name = Resources.Model.Strings.CentralBankKeys.FromDateTime)]
        #endregion
        public string FromDateTime { get; set; }

        #region ToDateTime
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.CentralBank),
           Name = Resources.Model.Strings.CentralBankKeys.ToDateTime)]
        #endregion
        public string ToDateTime { get; set; }

        #region PaymentIdentifier
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.CentralBank),
           Name = Resources.Model.Strings.CentralBankKeys.PaymentIdentifier)]
        #endregion
        public string PaymentIdentifier { get; set; }

        #region PageNumber
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.CentralBank),
           Name = Resources.Model.Strings.CentralBankKeys.PageNumber)]
        #endregion
        public int PageNumber { get; set; }

        #region RecordCount
        [System.ComponentModel.DataAnnotations.Display
           (ResourceType = typeof(Resources.Model.CentralBank),
           Name = Resources.Model.Strings.CentralBankKeys.RecordCount)]
        #endregion
        public int RecordCount { get; set; }
        public int transactionType { get; set; }
        public double amount { get; set; }
        public long? traceNumber { get; set; }
        public string transactionDate { get; set; }

    }
}
