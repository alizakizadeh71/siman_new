using System.Collections.Generic;

namespace Models
{
    public class Province : BaseExtendedEntity
	{
		#region Configuration
		internal class Configuration :
            System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Province>
		{
			public Configuration()
			{
                Property(current => current.Name).HasMaxLength(50);
                Property(current => current.Code).HasMaxLength(10);
                Property(current => current.BankCode).HasMaxLength(2);
			}
		}
		#endregion /Configuration

        public Province()
		{
		}

        public string Name { get; set; }

        public string Code { get; set; }

        public string BankCode { get; set; }

        public virtual System.Collections.Generic.IList<City> Cities { get; set; }
        public virtual System.Collections.Generic.IList<AccountNumberManage> AccountNumberManages { get; set; }
        public virtual System.Collections.Generic.IList<Request> Requests { get; set; }
        public virtual System.Collections.Generic.IList<FactorCement> FactorCements { get; set; }
        public virtual System.Collections.Generic.IList<HeadOfFactor> HeadOfFactors { get; set; }
        public virtual System.Collections.Generic.IList<AccountNumber> AccountNumbers { get; set; }
        public virtual System.Collections.Generic.IList<User> Users { get; set; }
    }
}
