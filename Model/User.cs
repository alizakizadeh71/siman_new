namespace Models
{
    public class User : BaseExtendedEntity
    {
        #region Configuration
        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<User>
        {
            public Configuration()
            {
                Property(current => current.FullName).HasMaxLength(100);
                Property(current => current.UserName).HasMaxLength(20);
                Property(current => current.Password).HasMaxLength(200);

                HasRequired(current => current.Role)
                    .WithMany(role => role.Users)
                    .HasForeignKey(current => current.RoleId)
                    .WillCascadeOnDelete(false)
                    ;

                HasOptional(current => current.Province)
                    .WithMany(province => province.Users)
                    .HasForeignKey(current => current.ProvinceId)
                    .WillCascadeOnDelete(false)
                    ;
                HasOptional(current => current.City)
                    .WithMany(City => City.Users)
                    .HasForeignKey(current => current.CityId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion

        public User()
        {
            LastLoginDateTime = System.DateTime.Now;
        }

        public virtual Role Role { get; set; }

        public System.Guid RoleId { get; set; }

        public virtual Province Province { get; set; }

        public System.Guid? ProvinceId { get; set; }

        public virtual City City { get; set; }

        public System.Guid? CityId { get; set; }

        public int LoginCount { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }
        public string NationalCode { get; set; }
        public string BuyerMobile { get; set; }
        public int creditAmount { get; set; }
        public System.DateTime? BirthDay { get; set; }
        public int InitialCredit { get; set; }

        public string Password { get; set; }

        public System.DateTime LastLoginDateTime { get; set; }
        public bool IsApprovallicense { get; set; }
        public bool Authenticate { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string IdentityCertificateSerial { get; set; }
        public int AmountOfTonnagePurchased { get; set; }

        public virtual System.Collections.Generic.IList<Request> Requests { get; set; }
        public virtual System.Collections.Generic.IList<HeadOfFactor> HeadOfFactors { get; set; }
        public virtual System.Collections.Generic.IList<Message> Messages { get; set; }
        public virtual System.Collections.Generic.IList<FactorMessage> FactorMessages { get; set; }
        public virtual System.Collections.Generic.IList<UserLoginLog> UserLoginLogs { get; set; }
        public virtual System.Collections.Generic.IList<CurrencyUnit> CurrencyUnits { get; set; }
        public virtual System.Collections.Generic.IList<FactorCement> FactorCements { get; set; }
        public virtual System.Collections.Generic.IList<walletFactor> walletFactor { get; set; }
        public virtual System.Collections.Generic.IList<FinancialManagement> FinancialManagements { get; set; }
        public virtual System.Collections.Generic.IList<DestinationManagement> DestinationManagements { get; set; }
    }
}
