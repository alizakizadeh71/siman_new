namespace Models
{
	public abstract class BaseAreaControllerAction : BaseExtendedEntity
	{
        #region Configuration
        internal class Configuration :
            System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<BaseAreaControllerAction>
        {
            public Configuration()
            {
            }
        }
        #endregion /Configuration

		public BaseAreaControllerAction()
		{
		}

        //public bool IsAvailableInSourceCode { get; set; }
        //public bool IsVisibleJustForProgrammer { get; set; }
        //public string Name { get; set; }
        //public string DisplayName { get; set; }
        //public string Description { get; set; }
	}
}
