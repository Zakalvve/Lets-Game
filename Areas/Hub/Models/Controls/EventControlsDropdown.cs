namespace LetsGame.Areas.Hub.Models.Controls
{
    public class EventControlsDropdown
    {
        public EventControlsDropdown(EventButtonModel e, EventButtonModel d)
        {
            Edit = e;
            Delete = d;
        }
        public EventButtonModel Edit { get; set; }
        public EventButtonModel Delete { get; set; }
    }
}
