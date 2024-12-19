namespace Models
{
    /// <summary>
    /// سرفصل
    /// </summary>
    public class HeadLine : BaseExtendedEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<HeadLine>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(300);
                Property(current => current.Code).HasMaxLength(2);
            }
        }
        #endregion

        public HeadLine()
        { }

        public string Name { get; set; }

        public string Code { get; set; }

        public virtual System.Collections.Generic.IList<SubHeadLine> SubHeadLines { get; set; }
        public virtual System.Collections.Generic.IList<HeadOfFactor> HeadOfFactors { get; set; }
    }
}
