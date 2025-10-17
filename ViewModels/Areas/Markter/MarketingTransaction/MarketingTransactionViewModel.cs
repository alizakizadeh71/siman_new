using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Areas.Markter.MarketingTransaction
{
    public class MarketingTransactionViewModel
    {
        public MarketingTransactionViewModel()
        { }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.ProductName)]
        public string ProductNameString { get; set; }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.ProductType)]
        public string ProductTypeString { get; set; }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.PackageType)]
        public string PackageTypeString { get; set; }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.FactoryName)]
        public string FactoryNameString { get; set; }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.Tonnage)]
        public string Tonnage { get; set; }

        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.CommissionAmount)]
        public string CommissionAmount { get; set; }
    }
}
