namespace Models
{
    public class City : BaseExtendedEntity
    {
        #region Configuration
        internal class Configuration :
            System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<City>
        {
            public Configuration()
            {
                Property(current => current.Name).HasMaxLength(50);
                Property(current => current.Code).HasMaxLength(10);

                HasRequired(current => current.Province)
                    .WithMany(user => user.Cities)
                    .HasForeignKey(current => current.ProvinceId)
                    .WillCascadeOnDelete(false)
                    ;

            }
        }
        #endregion /Configuration

        public City()
        {
        }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.City),
            Name = Resources.Model.Strings.CityKeys.Province)]
        public virtual Province Province { get; set; }

        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.City),
            Name = Resources.Model.Strings.CityKeys.Province)]
        public System.Guid ProvinceId { get; set; }


        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.City),
            Name = Resources.Model.Strings.CityKeys.Name)]
        public string Name { get; set; }


        [System.ComponentModel.DataAnnotations.Display
            (ResourceType = typeof(Resources.Model.City),
            Name = Resources.Model.Strings.CityKeys.Code)]
        public string Code { get; set; }

        public virtual System.Collections.Generic.IList<Request> Requests { get; set; }
        public virtual System.Collections.Generic.IList<village> Villages { get; set; }
        //public virtual System.Collections.Generic.IList<FactorCement> FactorCements { get; set; }
        public virtual System.Collections.Generic.IList<HeadOfFactor> HeadOfFactors { get; set; }
        public virtual System.Collections.Generic.IList<User> Users { get; set; }
        public virtual System.Collections.Generic.IList<DestinationManagement> DestinationManagements { get; set; }
    }
}
