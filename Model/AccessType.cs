using System.Linq;
using System.Data.Entity;

namespace Models
{
    public class AccessType : BaseExtendedEntity
	{
		#region Configuration
		internal class Configuration :
			System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<AccessType>
		{
			public Configuration()
			{
                Property(current => current.Name).HasMaxLength(100);
                Property(current => current.Description).HasMaxLength(200);
			}
		}
		#endregion /Configuration

		public AccessType()
		{
		}

		public int Code { get; set; }
		
		public Enums.AccessTypes CodeEnum
		{
			get
			{
				return ((Enums.AccessTypes)Code);
			}
		}
		
		public string Name { get; set; }
		
		public string Description { get; set; }
		
		public virtual System.Collections.Generic.IList<ProjectAction> ProjectActions { get; set; }
	}
}
