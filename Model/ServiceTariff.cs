namespace Models
{
    /// <summary>
    /// تعرفه خدمات
    /// </summary>
    public class ServiceTariff : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ServiceTariff>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(400);
                Property(current => current.VCode).HasMaxLength(6);
                Property(current => current.RCode).HasMaxLength(3);


                HasOptional(current => current.BankAccount)
                    .WithMany(bankaccount => bankaccount.ServiceTariffs)
                    .HasForeignKey(current => current.BankAccountId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.Unit)
                    .WithMany(unit => unit.ServiceTariffs)
                    .HasForeignKey(current => current.UnitId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.SubHeadLine)
                    .WithMany(unit => unit.ServiceTariffs)
                    .HasForeignKey(current => current.SubHeadLineId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion

        public ServiceTariff()
        { }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.Name)]
        public string Name { get; set; }
        public string NameString {
			get
			{
				return this.Name + " - " + this.Amount;
			}
		}


        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.VCode)]
        public string VCode { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.RCode)]
        public string RCode { get; set; }


        /// <summary>
        /// عنوان حساب - شامل : تمامی اطلاعات پایه برای کد 17 رقمی
        /// </summary>
        #region OfficeService
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.BankAccount)]
        #endregion
        public virtual BankAccount BankAccount { get; set; }

        /// <summary>
        /// عنوان حساب - شامل : تمامی اطلاعات پایه برای کد 17 رقمی
        /// </summary>
        #region OfficeServiceId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.BankAccount)]
        #endregion
        public System.Guid? BankAccountId { get; set; }



        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.Unit)]
        public virtual Unit Unit { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.Unit)]
        public System.Guid UnitId { get; set; }


        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.SubHeadLine)]
        public virtual SubHeadLine SubHeadLine { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.SubHeadLine)]

        public System.Guid SubHeadLineId { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.ServiceTariff),
            Name = Resources.Model.Strings.ServiceTariffKeys.Amount)]
        public decimal Amount { get; set; }

        public virtual System.Collections.Generic.IList<Request> Requests { get; set; }
        public virtual System.Collections.Generic.IList<DetailOfFactor> DetailOfFactors { get; set; }
        public virtual System.Collections.Generic.IList<CommodityInSubSystem> CommodityInSubSystems { get; set; }
        public virtual System.Collections.Generic.IList<ServiceTariffInSubSystem> ServiceTariffInSubSystems { get; set; }
    }
}
