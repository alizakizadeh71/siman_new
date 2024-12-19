namespace Models
{
    public class village : BaseExtendedEntity
    {
        #region Configuration
        internal class Configuration :
            System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<village>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(50);
                Property(current => current.Code).HasMaxLength(10);

                HasRequired(current => current.City)
                    .WithMany(user => user.Villages)
                    .HasForeignKey(current => current.Cityid)
                    .WillCascadeOnDelete(false)
                    ;

            }
        }
        #endregion /Configuration


        public village()
        {
        }

        [System.ComponentModel.DataAnnotations.Display
    (ResourceType = typeof(Resources.Model.village),
    Name = Resources.Model.Strings.villageKeys.City)]
        public virtual City City { get; set; }

        [System.ComponentModel.DataAnnotations.Display
    (ResourceType = typeof(Resources.Model.village),
    Name = Resources.Model.Strings.villageKeys.City)]
        public System.Guid Cityid { get; set; }

        [System.ComponentModel.DataAnnotations.Display
(ResourceType = typeof(Resources.Model.village),
Name = Resources.Model.Strings.villageKeys.Province)]
        public System.Guid ProvinceId { get; set; }


        [System.ComponentModel.DataAnnotations.Display
    (ResourceType = typeof(Resources.Model.village),
    Name = Resources.Model.Strings.villageKeys.Name)]
        public string Name { get; set; }


        [System.ComponentModel.DataAnnotations.Display
    (ResourceType = typeof(Resources.Model.village),
    Name = Resources.Model.Strings.villageKeys.Code)]
        public string Code { get; set; }

        public virtual System.Collections.Generic.IList<DestinationManagement> DestinationManagements { get; set; }
    }
}
