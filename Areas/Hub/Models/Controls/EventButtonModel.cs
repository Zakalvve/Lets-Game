namespace LetsGame.Areas.Hub.Models.Controls
{
    public class EventButtonModel : BackLink
    {
        public EventButtonModel(long? eventID, string returnURL) : this(eventID, returnURL, ControlType.BUTTON) { }

		public EventButtonModel(long? eventID,string returnURL, ControlType ct) : base(returnURL) {
			EventID = eventID.HasValue ? (long)eventID : 0;
            ControlType = ct;
		}

        public EventButtonModel(string returnURL) : this(0, returnURL) { }

        public ControlType ControlType { get; set; }

        public long EventID { get; private set; }

        public bool IsCreate
        {
            get
            {
                return EventID == 0;
            }
        }
    }

    public enum ControlType {
        BUTTON,
        DROP_DOWN
    }
}
