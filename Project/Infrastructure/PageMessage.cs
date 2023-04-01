namespace Infrastructure
{
	public class PageMessage : System.Object
	{
		public PageMessage (Enums.PageMessageTypes type, string message)
		{
			Type = type;
			Message = message;
		}

		public string Message { get; set; }
        public Enums.PageMessageTypes Type { get; set; }
	}
}
