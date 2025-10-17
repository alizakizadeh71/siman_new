using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public  class MarketerTransactions : BaseExtendedEntity
    {
        internal class
            Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<MarketerTransactions>
        {
            public Configuration()
            {
            }
        }

        public MarketerTransactions()
        {
        }
        #region MarketingCode
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.MarketingCode)]
        #endregion
        public int MarketingCode { get; set; }
        #region ReferredCode
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.ReferredCode)]
        #endregion
        public int ReferredCode { get; set; }
        #region ProductName
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.ProductName)]
        #endregion
        public Guid ProductNameId { get; set; }
        public virtual ProductName ProductName { get; set; }

        #region ProductType
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.ProductType)]
        #endregion
        public Guid ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }


        #region PackageType
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.PackageType)]
        #endregion
        public Guid PackageTypeId { get; set; }
        public virtual PackageType PackageType { get; set; }

        #region FactoryName
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.FactoryName)]
        #endregion
        public Guid FactoryNameId { get; set; }
        public virtual FactoryName FactoryName { get; set; }
        #region Tonnage
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.Tonnage)]
        #endregion
        public double Tonnagedouble { get; set; }
        #region Tonnage
        [System.ComponentModel.DataAnnotations.Display
        (ResourceType = typeof(Resources.Model.MarketerTransactions),
            Name = Resources.Model.Strings.MarketerTransactionsKeys.CommissionAmount)]
        #endregion
        public int CommissionAmount { get; set; }
    }
}
