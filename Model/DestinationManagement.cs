using System;

namespace Models
{
    public class DestinationManagement : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<DestinationManagement>
        {
            public Configuration()
            {
                //HasRequired(current => current.User)
                //     .WithMany(user => user.DestinationManagements)
                //     .HasForeignKey(current => current.UserId)
                //     .WillCascadeOnDelete(true)
                //     ;

                HasRequired(current => current.FinancialManagement)
                    .WithMany(x => x.DestinationManagements)
                    .HasForeignKey(current => current.FinancialManagementId)
                    .WillCascadeOnDelete(false)
                    ;

                HasOptional(current => current.Province)
                    .WithMany(x => x.DestinationManagements)
                    .HasForeignKey(current => current.ProvinceId)
                    .WillCascadeOnDelete(true)
                    ;

                HasRequired(current => current.City)
                    .WithMany(x => x.DestinationManagements)
                    .HasForeignKey(current => current.CityId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.City)
                .WithMany(x => x.DestinationManagements)
                .HasForeignKey(current => current.villageId)
                .WillCascadeOnDelete(false)
                ;

            }
        }

        #endregion

        public DestinationManagement()
        {
        }

        //#region User
        //[System.ComponentModel.DataAnnotations.Display
        //    (ResourceType = typeof(Resources.Model.Request),
        //    Name = Resources.Model.Strings.RequestKeys.User)]
        //#endregion
        //public virtual User User { get; set; }

        //#region UserId
        //[System.ComponentModel.DataAnnotations.Display
        //    (ResourceType = typeof(Resources.Model.Request),
        //    Name = Resources.Model.Strings.RequestKeys.User)]
        //#endregion
        //public System.Guid UserId { get; set; }

        public Guid FinancialManagementId { get; set; }
        public virtual FinancialManagement FinancialManagement { get; set; }


        #region Province
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.Province)]
        #endregion
        public Guid? ProvinceId { get; set; }
        public virtual Province Province { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
             (ResourceType = typeof(Resources.Model.Cement),
             Name = Resources.Model.Strings.CementKeys.City)]
        #endregion
        public Guid CityId { get; set; }
        public virtual City City { get; set; }

        public Guid? villageId { get; set; }
        public virtual village Village { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.Request),
            Name = Resources.Model.Strings.RequestKeys.AmountPaid)]
        #endregion
        public long DestinationAmountPaid { get; set; }
    }
}
