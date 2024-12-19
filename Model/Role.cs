using System.Linq;

namespace Models
{
    public class Role : BaseExtendedEntity
    {
        #region Configuration
        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Role>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(30);
                Property(current => current.Description).HasMaxLength(100);
            }
        }
        #endregion

        public static Role GetByCultureAndCode(Enums.Roles code)
        {
            Role oResult = null;
            DatabaseContext oDatabaseContext = null;

            try
            {
                oDatabaseContext =
                    new DatabaseContext();

                oResult =
                    oDatabaseContext.Roles
                    .Where(current => current.Code == (int)code)
                    .FirstOrDefault();
            }
            catch
            {
            }
            finally
            {
                if (oDatabaseContext != null)
                {
                    oDatabaseContext.Dispose();
                    oDatabaseContext = null;
                }
            }

            return (oResult);
        }

        public Role()
        { }

        public int Code { get; set; }

        public Enums.Roles CodeEnum
        {
            get
            {
                return ((Enums.Roles)Code);
            }
        }

        public string Name { get; set; }

        [System.Web.Mvc.AllowHtml]
        [System.ComponentModel.DataAnnotations.DataType
        (System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public string Description { get; set; }

        public virtual System.Collections.Generic.IList<User> Users { get; set; }
        public virtual System.Collections.Generic.IList<ProjectAction> ProjectActions { get; set; }
    }
}
