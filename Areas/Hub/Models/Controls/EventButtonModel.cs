namespace LetsGame.Areas.Hub.Models.Controls
{
    public class EventButtonModel : BackLink
    {
        public EventButtonModel(long? eventID, string returnURL) : base(returnURL)
        {
            EventID = eventID.HasValue ? (long)eventID : 0;
        }
        public EventButtonModel(string returnURL) : this(0, returnURL) { }

        public long EventID { get; private set; }

        public bool IsCreate
        {
            get
            {
                return EventID == 0;
            }
        }
    }
}
