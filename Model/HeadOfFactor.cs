using System.Linq;
using System.Data.Entity;

namespace Models
{
    public class HeadOfFactor : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<HeadOfFactor>
        {
            public Configuration()
            {
                Property(current => current.InvoiceNumber)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
                
                Property(current => current.CompanyName).HasMaxLength(100);
                Property(current => current.CompanyNationalCode).HasMaxLength(11);
                Property(current => current.CellPhoneNumber).HasMaxLength(11);

                HasRequired(current => current.HeadLine)
                    .WithMany(HeadLine => HeadLine.HeadOfFactors)
                    .HasForeignKey(current => current.HeadLineId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.SubHeadLine)
                    .WithMany(subHeadLine => subHeadLine.HeadOfFactors)
                    .HasForeignKey(current => current.SubHeadLineId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.User)
                    .WithMany(user => user.HeadOfFactors)
                    .HasForeignKey(current => current.UserId)
                    .WillCascadeOnDelete(false)
                    ;

                HasRequired(current => current.Province)
                    .WithMany(user => user.HeadOfFactors)
                    .HasForeignKey(current => current.ProvinceId)
                    .WillCascadeOnDelete(false)
                    ;

                HasOptional(current => current.Request)
                    .WithOptionalPrincipal(user => user.HeadOfFactor)
                    .Map(x => x.MapKey("HeadOfFactorId"))
                    ;

                HasOptional(current => current.City)
                    .WithMany(user => user.HeadOfFactors)
                    .HasForeignKey(current => current.CityId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }

        #endregion

        public HeadOfFactor()
        {
        }

        /// <summary>
        /// درخواست
        /// </summary>
        #region Request
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.Request)]
        #endregion
        public virtual Request Request { get; set; }
        

        /// <summary>
        /// سرفصل
        /// </summary>
        #region OfficeService
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.HeadLine)]
        #endregion
        public virtual HeadLine HeadLine { get; set; }

        #region HeadLineId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.HeadLine)]
        #endregion
        public System.Guid HeadLineId { get; set; }

        /// <summary>
        /// زیر فصل
        /// </summary>
        #region OfficeService
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.SubHeadLine)]
        #endregion
        public virtual SubHeadLine SubHeadLine { get; set; }

        #region SubHeadLineId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.SubHeadLine)]
        #endregion
        public System.Guid SubHeadLineId { get; set; }
        
        #region User
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.User)]
        #endregion
        public virtual User User { get; set; }

        #region UserId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.User)]
        #endregion
        public System.Guid UserId { get; set; }

        #region Province
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.Province)]
        #endregion
        public virtual Province Province { get; set; }

        #region ProvinceId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.Province)]
        #endregion
        public System.Guid ProvinceId { get; set; }

        #region City
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.City)]
        #endregion
        public virtual City City { get; set; }

        #region CityId
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.City)]
        #endregion
        public System.Guid? CityId { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CompanyName)]
        #endregion
        public string CompanyName { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CompanyNationalCode)]
        #endregion
        public string CompanyNationalCode { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.InvoiceNumber)]
        #endregion
        public int InvoiceNumber { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.InvoiceDate)]
        #endregion
        public System.DateTime InvoiceDate { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.CellPhoneNumber)]
        #endregion
        public string CellPhoneNumber { get; set; }

        #region
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.Description)]
        #endregion
        public string Description { get; set; }

        #region FinalApprove
        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.HeadOfFactor),
            Name = Resources.Model.Strings.HeadOfFactorKeys.FinalApprove)]
        #endregion
        public bool FinalApprove { get; set; }

        public virtual System.Collections.Generic.IList<FactorMessage> FactorMessages { get; set; }
        public virtual System.Collections.Generic.IList<DetailOfFactor> DetailOfFactors { get; set; }

    }
}
