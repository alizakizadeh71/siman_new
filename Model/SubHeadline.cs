namespace Models
{
    /// <summary>
    /// زیر فصل
    /// </summary>
    public class SubHeadLine : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SubHeadLine>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(400);
                Property(current => current.Code).HasMaxLength(2);

                HasRequired(current => current.HeadLine)
                    .WithMany(office => office.SubHeadLines)
                    .HasForeignKey(current => current.HeadLineId)
                    .WillCascadeOnDelete(false)
                    ;
            }
        }
        #endregion

        public SubHeadLine()
        { }

        public string Name { get; set; }

        public string Code { get; set; }


        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.SubHeadLine),
            Name = Resources.Model.Strings.SubHeadLineKeys.HeadLine)]
        public virtual HeadLine HeadLine { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.SubHeadLine),
            Name = Resources.Model.Strings.SubHeadLineKeys.HeadLine)]
        public System.Guid HeadLineId { get; set; }


        public virtual System.Collections.Generic.IList<ServiceTariff> ServiceTariffs { get; set; }
        public virtual System.Collections.Generic.IList<HeadOfFactor> HeadOfFactors { get; set; }
        //public virtual System.Collections.Generic.IList<PackageType> PackageTypes { get; set; }
    }
}
