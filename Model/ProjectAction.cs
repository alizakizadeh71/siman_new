namespace Models
{
	public class ProjectAction : BaseAreaControllerAction
	{
		#region Configuration
		internal class Configuration :
			System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ProjectAction>
		{
			public Configuration()
			{
                Property(current => current.Action).HasMaxLength(50);
                Property(current => current.Area).HasMaxLength(50);
                Property(current => current.Controller).HasMaxLength(50);


                HasMany(current => current.Roles)
                    .WithMany(role => role.ProjectActions)
                    .Map(current =>
                    {
                        current.ToTable("ProjectActionsOfRoles");
                        // MapRightKey را می نويسيم و بعد MapLeftKey اول
                        // و سپس قانون دور در دور و نزديک در نزديک را رعايت می کنيم
                        current.MapLeftKey("ProjectActionId");
                        current.MapRightKey("RoleId");
                    });
			}
		}
		#endregion /Configuration

		public ProjectAction()
		{
		}

        public bool IsPublic { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public virtual System.Collections.Generic.IList<Role> Roles { get; set; }
	}
}
